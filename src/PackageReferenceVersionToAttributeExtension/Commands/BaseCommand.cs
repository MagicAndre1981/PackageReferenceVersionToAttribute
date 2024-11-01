// <copyright file="BaseCommand.cs" company="Rami Abughazaleh">
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
    using Microsoft.VisualStudio.Shell;
    using PackageReferenceVersionToAttributeExtension.Services;

    /// <summary>
    /// Base command.
    /// </summary>
    internal class BaseCommand(
        LoggingService loggingService,
        ProjectService projectService,
        FileSystemService fileSystemService)
    {
        private readonly LoggingService loggingService = loggingService;
        private readonly ProjectService projectService = projectService;
        private readonly FileSystemService fileSystemService = fileSystemService;

        /// <summary>
        /// Converts all &lt;Version&gt; elements within &lt;PackageReference&gt; items in the project file
        /// to attributes, ensuring that version information is consistently represented as an attribute.
        /// This operation scans for &lt;PackageReference&gt; elements, identifies child &lt;Version&gt;
        /// elements, and moves the version value to an attribute if found.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        internal async Task ConvertPackageReferenceVersionElementsToAttributesAsync()
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

        /// <summary>
        /// Retrieves the projects currently selected in Solution Explorer.
        /// This method identifies selected project nodes, as well as projects under a solution node if it is selected.
        /// </summary>
        /// <returns>A collection of distinct projects selected in Solution Explorer.</returns>
        internal async Task<IEnumerable<Project>> GetSelectedProjectsAsync()
        {
            // Get the active items from Solution Explorer
            var activeItems = await VS.Solutions.GetActiveItemsAsync();

            // A HashSet to ensure unique projects are returned
            var projects = new HashSet<Project>();

            foreach (var item in activeItems)
            {
                if (item is Solution solution)
                {
                    // Get all projects in the solution
                    projects.UnionWith(this.GetAllProjects(solution.Children));
                }
                else if (item is Project project)
                {
                    // Directly add selected projects
                    projects.Add(project);
                }
            }

            return projects;
        }

        private async Task ConvertPackageReferenceVersionElementsToAttributesAsync(IEnumerable<Community.VisualStudio.Toolkit.Project> projects)
        {
            foreach (var project in projects)
            {
                try
                {
                    var projectPath = project.FullPath;
                    if (string.IsNullOrEmpty(projectPath) || !File.Exists(projectPath))
                    {
                        continue;
                    }

                    string message = $"Converting PackageReference Version child elements to attributes in the project file \"{projectPath}\"...";
                    await this.loggingService.LogInfoAsync(message);
                    await VS.StatusBar.ShowMessageAsync(message);

                    var document = XDocument.Load(projectPath, LoadOptions.PreserveWhitespace);

                    // Find all PackageReference elements with a <Version> child element
                    // Use the XML namespace if one is set
                    XNamespace ns = document.Root.GetDefaultNamespace();

                    if (!string.IsNullOrWhiteSpace(ns.NamespaceName))
                    {
                        await this.loggingService.LogInfoAsync($"Found XML namespace \"{ns.NamespaceName}\".");
                    }

                    // Query for PackageReference elements
                    var packageReferences = document.Descendants(ns != null ? ns + "PackageReference" : "PackageReference")
                        .Where(pr => pr.Element(ns != null ? ns + "Version" : "Version") != null)
                        .ToList();
                    if (!packageReferences.Any())
                    {
                        await this.loggingService.LogInfoAsync($"No PackageReference Version child elements found in the project file \"{projectPath}\".");
                        continue;
                    }

                    bool modified = false;

                    // backup project file
                    await this.fileSystemService.BackupFileAsync(projectPath);

                    // check out files from source control
                    await this.projectService.CheckOutFileFromSourceControlAsync(projectPath);

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
                            OmitXmlDeclaration = true,
                            Indent = false, // Prevents adding any extra indentation
                            NewLineHandling = NewLineHandling.Replace,
                        };

                        using var writer = XmlWriter.Create(projectPath, settings);

                        document.Save(writer); // Preserves original formatting, avoids extra lines
                    }
                }
                catch (Exception ex)
                {
                    await this.loggingService.LogErrorAsync(ex);
                }
            }
        }

        private IEnumerable<Project> GetAllProjects(
            IEnumerable<SolutionItem> items)
        {
            var projects = new List<Project>();

            foreach (var item in items)
            {
                this.AddProjectsRecursively(item, projects);
            }

            return projects;
        }

        // Recursive method to gather projects from solution and solution folders
        private void AddProjectsRecursively(SolutionItem solutionItem, List<Project> projects)
        {
            if (solutionItem is Project project)
            {
                projects.Add(project);
            }
            else if (solutionItem is SolutionFolder folder)
            {
                // Recursively add projects within each solution folder
                foreach (var child in folder.Children)
                {
                    this.AddProjectsRecursively(child, projects);
                }
            }
        }
    }
}
