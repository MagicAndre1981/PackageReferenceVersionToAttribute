// <copyright file="MockVisualStudio.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeExtensionTests.Mocks
{
    using System;
    using EnvDTE;
    using Microsoft.VisualStudio.ComponentModelHost;
    using Microsoft.VisualStudio.Sdk.TestFramework;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;
    using PackageReferenceVersionToAttributeExtensionTests.Setup;

    /// <summary>
    /// Mock Visual Studio.
    /// </summary>
    internal sealed class MockVisualStudio : IDisposable
    {
        private readonly MockHierarchyItemManager hierarchyItemManager;
        private readonly MockMultiItemSelect multiItemSelect;
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="MockVisualStudio"/> class.
        /// </summary>
        /// <param name="globalServiceProvider">The global service provider.</param>
        /// <param name="serviceProvider">The service provider.</param>
        public MockVisualStudio(
            GlobalServiceProvider globalServiceProvider,
            DisposableServiceProvider serviceProvider)
        {
            this.multiItemSelect = serviceProvider.GetRequiredService<MockMultiItemSelect>();

            globalServiceProvider.AddService(
                typeof(DTE),
                serviceProvider.GetRequiredService<MockDevelopmentToolsEnvironment>());

            globalServiceProvider.AddService(
                typeof(IVsStatusbar),
                serviceProvider.GetRequiredService<MockStatusbar>());

            globalServiceProvider.AddService(
                typeof(SVsSolution),
                serviceProvider.GetRequiredService<MockSolution>());

            globalServiceProvider.AddService(
                typeof(SVsGeneralOutputWindowPane),
                serviceProvider.GetRequiredService<MockOutputWindowPane>());

            globalServiceProvider.AddService(
                typeof(SVsOutputWindow),
                serviceProvider.GetRequiredService<MockOutputWindow>());

            this.hierarchyItemManager = serviceProvider.GetRequiredService<MockHierarchyItemManager>();
            globalServiceProvider.AddService(typeof(IVsHierarchyItemManager), this.hierarchyItemManager);

            MockComponentModel componentModel = serviceProvider.GetRequiredService<MockComponentModel>();
            componentModel.AddService<IVsHierarchyItemManager>(this.hierarchyItemManager);
            globalServiceProvider.AddService(typeof(SComponentModel), componentModel);

            globalServiceProvider.AddService(
                typeof(IVsMonitorSelection),
                serviceProvider.GetRequiredService<MockMonitorSelection>());
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;
        }

        /// <summary>
        /// Adds the projects to the hierarchy.
        /// </summary>
        /// <param name="projects">The projects.</param>
        internal void AddProjects(params MockProject[] projects)
        {
            this.hierarchyItemManager.AddItems(projects);
        }

        /// <summary>
        /// Adds the items to the selection.
        /// </summary>
        /// <param name="items">The items.</param>
        internal void AddSelections(params MockProject[] items)
        {
            this.multiItemSelect.AddSelections(items);
        }
    }
}
