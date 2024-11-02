// <copyright file="ProjectConverter.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttribute
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Linq;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Provides functionality to convert project files, modifying elements and attributes as required.
    /// </summary>
    public class ProjectConverter
    {
        private readonly ILogger<ProjectConverter> logger;
        private readonly IFileService fileService;
        private readonly ISourceControlService sourceControlService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectConverter"/> class.
        /// </summary>
        /// <param name="logger">The logger used to log information and errors during the conversion process.</param>
        /// <param name="fileService">Service for file operations, such as backing up and modifying file attributes.</param>
        /// <param name="sourceControlService">Service for source control operations, such as checking out files.</param>
        public ProjectConverter(
            ILogger<ProjectConverter> logger,
            IFileService fileService,
            ISourceControlService sourceControlService)
        {
            this.logger = logger;
            this.fileService = fileService;
            this.sourceControlService = sourceControlService;
        }

        /// <summary>
        /// Converts the specified project files by modifying elements and attributes as required.
        /// </summary>
        /// <param name="projectFilePaths">A collection of paths to the project files to be converted.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task ConvertAsync(IEnumerable<string> projectFilePaths)
        {
            foreach (var projectFilePath in projectFilePaths)
            {
                try
                {
                    if (string.IsNullOrEmpty(projectFilePath) || !File.Exists(projectFilePath))
                    {
                        continue;
                    }

                    string message = $"Converting PackageReference Version child elements to attributes in the project file \"{projectFilePath}\"...";
                    this.logger.LogInformation(message);

                    var document = XDocument.Load(projectFilePath, LoadOptions.PreserveWhitespace);

                    // Find all PackageReference elements with a <Version> child element
                    // Use the XML namespace if one is set
                    XNamespace ns = document.Root.GetDefaultNamespace();

                    if (!string.IsNullOrWhiteSpace(ns.NamespaceName))
                    {
                        this.logger.LogInformation($"Found XML namespace \"{ns.NamespaceName}\".");
                    }

                    // Query for PackageReference elements
                    var packageReferences = document.Descendants(ns != null ? ns + "PackageReference" : "PackageReference")
                        .Where(pr => pr.Element(ns != null ? ns + "Version" : "Version") != null)
                        .ToList();
                    if (!packageReferences.Any())
                    {
                        this.logger.LogInformation($"No PackageReference Version child elements found in the project file \"{projectFilePath}\".");
                        continue;
                    }

                    bool modified = false;

                    // backup project file
                    this.fileService.BackupFile(projectFilePath);

                    // check out file from source control
                    await this.sourceControlService.CheckOutFileAsync(projectFilePath);

                    foreach (var packageReference in packageReferences)
                    {
                        var versionElement = packageReference.Element(ns != null ? ns + "Version" : "Version");
                        if (versionElement != null)
                        {
                            // Move the Version element content to an attribute
                            packageReference.SetAttributeValue("Version", versionElement.Value);
                            versionElement.Remove();

                            // Check if the PackageReference is empty and set it to self-closing if so
                            if (!packageReference.HasElements)
                            {
                                // This will make sure it's treated as a self-closing tag when saved
                                // Optionally, trim empty lines around the parent element
                                packageReference.RemoveNodes(); // Remove empty nodes
                            }

                            modified = true;
                        }
                    }

                    // Save changes if any modifications were made
                    if (modified)
                    {
                        var settings = new XmlWriterSettings
                        {
                            OmitXmlDeclaration = document.Declaration == null, // Preserve the XML declaration if it exists.
                            Indent = false,                                    // Prevents adding any extra indentation
                            NewLineHandling = NewLineHandling.Replace,
                        };

                        using var writer = XmlWriter.Create(projectFilePath, settings);

                        document.Save(writer); // Preserves original formatting, avoids extra lines
                    }
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, $"Error processing project file \"{projectFilePath}\"");
                }
            }
        }
    }
}
