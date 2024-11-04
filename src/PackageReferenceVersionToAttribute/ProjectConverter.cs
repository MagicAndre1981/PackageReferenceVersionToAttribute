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
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Provides functionality to convert project files, modifying elements and attributes as required.
    /// </summary>
    public class ProjectConverter
    {
        private readonly ILogger<ProjectConverter> logger;
        private readonly IFileService fileService;
        private readonly ISourceControlService sourceControlService;
        private readonly ProjectConverterOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectConverter"/> class.
        /// </summary>
        /// <param name="logger">The logger used to log information and errors during the conversion process.</param>
        /// <param name="fileService">Service for file operations, such as backing up and modifying file attributes.</param>
        /// <param name="sourceControlService">Service for source control operations, such as checking out files.</param>
        /// <param name="options">The options.</param>
        public ProjectConverter(
            ILogger<ProjectConverter> logger,
            IFileService fileService,
            ISourceControlService sourceControlService,
            IOptions<ProjectConverterOptions> options)
        {
            this.logger = logger;
            this.fileService = fileService;
            this.sourceControlService = sourceControlService;
            this.options = options.Value;
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
                    await this.ConvertAsync(projectFilePath);
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, $"Error processing project file \"{projectFilePath}\"");
                }
            }
        }

        private async Task ConvertAsync(string projectFilePath)
        {
            if (string.IsNullOrEmpty(projectFilePath) || !File.Exists(projectFilePath))
            {
                return;
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
            var packageReferenceVersionElements = document.Descendants(ns != null ? ns + "PackageReference" : "PackageReference")
                .Where(pr => pr.Element(ns != null ? ns + "Version" : "Version") != null)
                .ToList();
            if (!packageReferenceVersionElements.Any())
            {
                this.logger.LogInformation($"No PackageReference Version child elements found in the project file \"{projectFilePath}\".");
                return;
            }

            bool modified = false;

            foreach (var packageReferenceVersionElement in packageReferenceVersionElements)
            {
                var versionElement = packageReferenceVersionElement.Element(ns != null ? ns + "Version" : "Version");
                if (versionElement != null)
                {
                    // Move the Version element content to an attribute
                    packageReferenceVersionElement.SetAttributeValue("Version", versionElement.Value);
                    versionElement.Remove();

                    // remove empty child nodes within the <PackageReference>
                    var childNodes = packageReferenceVersionElement.Nodes()
                        .OfType<XText>()
                        .Where(x => string.IsNullOrWhiteSpace(x.Value));
                    foreach (var node in childNodes)
                    {
                        node.Remove();
                    }

                    // Check if the PackageReference is empty and set it to self-closing if so
                    if (!packageReferenceVersionElement.HasElements)
                    {
                        // This will make sure it's treated as a self-closing tag when saved
                        // Optionally, trim empty lines around the parent element
                        packageReferenceVersionElement.RemoveNodes(); // Remove empty nodes
                    }

                    modified = true;
                }
            }

            // Save changes only if any modifications were made
            if (!modified)
            {
                return;
            }

            if (this.options.Backup)
            {
                // backup project file
                this.fileService.BackupFile(projectFilePath);
            }

            // check out file from source control
            await this.sourceControlService.CheckOutFileAsync(projectFilePath);

            if (this.options.Force)
            {
                this.fileService.RemoveReadOnlyAttribute(projectFilePath);
            }

            string detectedLineEnding = DetectLineEnding(projectFilePath);

            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = document.Declaration == null, // Preserve the XML declaration if it exists.
                Indent = false,                                    // Prevents adding any extra indentation.
                NewLineHandling = NewLineHandling.Replace,         // Preserve the same line endings.
                NewLineChars = detectedLineEnding,                 // Preserve the same line endings.
            };

            if (this.options.DryRun)
            {
                // Output the modified document to the console for review
                using var stringWriter = new StringWriter();
                using var xmlWriter = XmlWriter.Create(stringWriter, settings);

                document.WriteTo(xmlWriter);
                xmlWriter.Flush();
                this.logger.LogInformation(stringWriter.ToString());
            }
            else
            {
                using var writer = XmlWriter.Create(projectFilePath, settings);
                document.Save(writer); // Preserves original formatting, avoids extra lines
            }
        }

        public static string DetectLineEnding(string filePath)
        {
            using var reader = new StreamReader(filePath);
            int ch;
            char previousChar = '\0';

            while ((ch = reader.Read()) != -1)
            {
                char currentChar = (char)ch;

                if (currentChar == '\n')
                {
                    return previousChar == '\r' ? "\r\n" : "\n";
                }

                previousChar = currentChar;
            }

            // Default to system line ending if no line ending found
            return Environment.NewLine;
        }
    }
}
