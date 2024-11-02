// <copyright file="BaseCommand.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeExtension
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Community.VisualStudio.Toolkit;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.Shell;
    using PackageReferenceVersionToAttribute;

    /// <summary>
    /// Base command.
    /// </summary>
    internal class BaseCommand(
        ILogger<BaseCommand> logger,
        ProjectConverter projectConverter)
    {
        private readonly ILogger<BaseCommand> logger = logger;
        private readonly ProjectConverter projectConverter = projectConverter;

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
                    this.logger.LogError("No projects selected.");
                    await VS.MessageBox.ShowWarningAsync("No project selected.");
                    return;
                }

                await VS.StatusBar.StartAnimationAsync(StatusAnimation.General);

                await this.projectConverter.ConvertAsync(
                    selectedProjects.Select(x => x.FullPath));

                this.logger.LogInformation($"Conversion completed successfully.");
                await VS.StatusBar.ShowMessageAsync("Conversion completed successfully.");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Conversion failed.");
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
