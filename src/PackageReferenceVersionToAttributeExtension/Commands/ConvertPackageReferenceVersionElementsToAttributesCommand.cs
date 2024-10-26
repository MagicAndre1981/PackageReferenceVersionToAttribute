// <copyright file="ConvertPackageReferenceVersionElementsToAttributesCommand.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeExtension
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Linq;
    using Community.VisualStudio.Toolkit;
    using Community.VisualStudio.Toolkit.DependencyInjection;
    using Community.VisualStudio.Toolkit.DependencyInjection.Core;
    using EnvDTE;
    using Microsoft.VisualStudio.Shell;
    using PackageReferenceVersionToAttributeExtension.Services;

    /// <summary>
    /// Convert PackageReference Version elements to attributes command.
    /// </summary>
    [Command(
        PackageGuids.guidPackageReferenceVersionToAttributeExtensionCmdSetString,
        PackageIds.PackageReferenceVersionToAttributeCommand)]
    internal sealed class ConvertPackageReferenceVersionElementsToAttributesCommand(
        DIToolkitPackage package,
        LoggingService loggingService,
        ProjectService projectService,
        FileSystemService fileSystemService) : BaseDICommand(package)
    {
        private readonly LoggingService loggingService = loggingService;
        private readonly ProjectService projectService = projectService;
        private readonly FileSystemService fileSystemService = fileSystemService;

        /// <inheritdoc/>
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            try
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                var selectedProjects = await this.GetSelectedProjectsAsync();
                if (!selectedProjects.Any())
                {
                    await this.loggingService.LogErrorAsync("No projects selected.");
                    await VS.MessageBox.ShowWarningAsync("No project selected.");
                    return;
                }

                await VS.StatusBar.StartAnimationAsync(StatusAnimation.General);

                await this.ConvertPackageReferenceVersionElementsToAttributesAsync(selectedProjects);

                await this.loggingService.LogInfoAsync($"Conversion completed successfully.");
                await VS.StatusBar.ShowMessageAsync("Conversion completed successfully.");
            }
            catch (Exception ex)
            {
                await this.loggingService.LogErrorAsync(ex);
                await VS.StatusBar.ShowMessageAsync("Conversion failed. Please see the Output Window for details.");
            }
            finally
            {
                await VS.StatusBar.EndAnimationAsync(StatusAnimation.General);
            }
        }

        private async Task ConvertPackageReferenceVersionElementsToAttributesAsync(IEnumerable<Community.VisualStudio.Toolkit.Project> projects)
        {
            foreach (var project in projects)
            {
                var projectPath = project.FullPath;
                if (string.IsNullOrEmpty(projectPath) || !File.Exists(projectPath))
                {
                    continue;
                }

                await VS.StatusBar.ShowMessageAsync($"Converting PackageReference Version child elements to attributes in {Path.GetFileName(projectPath)}...");

                // backup project file
                await this.fileSystemService.BackupFileAsync(projectPath);

                // check out files from source control
                await this.projectService.CheckOutFileFromSourceControlAsync(projectPath);

                var document = XDocument.Load(projectPath, LoadOptions.PreserveWhitespace);
                bool modified = false;

                // Find all PackageReference elements with a <Version> child element
                var packageReferences = document.Descendants("PackageReference")
                    .Where(pr => pr.Element("Version") != null)
                    .ToList();

                foreach (var packageReference in packageReferences)
                {
                    var versionElement = packageReference.Element("Version");
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
                        OmitXmlDeclaration = true,
                        Indent = false, // Prevents adding any extra indentation
                    };

                    using (var writer = XmlWriter.Create(projectPath, settings))
                    {
                        document.Save(writer); // Preserves original formatting, avoids extra lines
                    }
                }
            }
        }

        private async Task<IEnumerable<Community.VisualStudio.Toolkit.Project>> GetSelectedProjectsAsync()
        {
            // Get the active items from the Solution Explorer
            var activeItems = await VS.Solutions.GetActiveItemsAsync();

            // Get the selected items of type Project
            return activeItems.OfType<Community.VisualStudio.Toolkit.Project>();
        }
    }
}
