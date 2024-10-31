// <copyright file="MockVisualStudio.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeExtensionTests.Mocks
{
    using EnvDTE;
    using Microsoft.VisualStudio.ComponentModelHost;
    using Microsoft.VisualStudio.Sdk.TestFramework;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;

    /// <summary>
    /// Mock Visual Studio.
    /// </summary>
    internal class MockVisualStudio
    {
        private MockHierarchyItemManager hierarchyItemManager;
        private MockMonitorSelection monitorSelection;
        private MockMultiItemSelect multiItemSelect;

        /// <summary>
        /// Initializes a new instance of the <see cref="MockVisualStudio"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public MockVisualStudio(GlobalServiceProvider serviceProvider)
        {
            MockHierarchy hierarchy = new();
            this.multiItemSelect = new MockMultiItemSelect(hierarchy);

            MockProjects projects = new();
            MockDteSolution dteSolution = new(projects);
            MockDevelopmentToolsEnvironment dte = new(dteSolution);
            serviceProvider.AddService(typeof(DTE), dte);

            MockOutputWindowPane outputWindowPane = new();
            serviceProvider.AddService(typeof(SVsGeneralOutputWindowPane), outputWindowPane);

            MockOutputWindow outputWindow = new(outputWindowPane);
            serviceProvider.AddService(typeof(SVsOutputWindow), outputWindow);

            this.monitorSelection = new(this.multiItemSelect, hierarchy);
            serviceProvider.AddService(typeof(IVsMonitorSelection), this.monitorSelection);

            MockComponentModel componentModel = new();
            serviceProvider.AddService(typeof(SComponentModel), componentModel);

            MockHierarchyItemIdentity hierarchyItemIdentity = new(hierarchy);
            MockHierarchyItem hierarchyItem = new(hierarchyItemIdentity);

            this.hierarchyItemManager = new(hierarchyItem);
            serviceProvider.AddService(typeof(IVsHierarchyItemManager), this.hierarchyItemManager);

            componentModel.AddService<IVsHierarchyItemManager>(this.hierarchyItemManager);

            MockSolution solution = new();
            serviceProvider.AddService(typeof(SVsSolution), solution);

            MockStatusbar stausbar = new MockStatusbar();
            serviceProvider.AddService(typeof(IVsStatusbar), stausbar);
        }

        /// <summary>
        /// Adds the project to the hierarchy.
        /// </summary>
        /// <param name="project">The project.</param>
        public void AddProject(MockProject project)
        {
            this.hierarchyItemManager.AddItem(project);
        }

        /// <summary>
        /// Adds the item to the selection.
        /// </summary>
        /// <param name="item">The item.</param>
        internal void AddSelection(MockProject item)
        {
            this.multiItemSelect.AddSelection(item);
        }
    }
}
