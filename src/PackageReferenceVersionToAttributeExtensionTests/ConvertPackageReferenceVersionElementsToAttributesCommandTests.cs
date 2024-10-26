// <copyright file="ConvertPackageReferenceVersionElementsToAttributesCommandTests.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeExtensionTests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Design;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using EnvDTE;
    using EnvDTE80;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.ComponentModelHost;
    using Microsoft.VisualStudio.Sdk.TestFramework;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using PackageReferenceVersionToAttributeExtension;
    using IAsyncServiceProvider = Microsoft.VisualStudio.Shell.Interop.COMAsyncServiceProvider.IAsyncServiceProvider;
    using OleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

    /// <summary>
    /// Convert PackageReference version elements to attributes command tests.
    /// </summary>
    [TestClass]
    public class ConvertPackageReferenceVersionElementsToAttributesCommandTests
    {
        private PackageReferenceVersionToAttributeExtensionPackage package;
        private MenuCommand command;
        private Mock<IVsMonitorSelection> mockMonitorSelection;
        private Mock<IVsMultiItemSelect> mockMultiItemSelect;
        private Mock<IVsHierarchy> mockHierarchy;

        private delegate void GetCurrentSelectionDelegate(out IntPtr ppHier, out uint pitemid, out IVsMultiItemSelect ppMIS, out IntPtr ppSC);

        private delegate void GetSelectionInfoDelegate(out uint pcItems, out int pfSingleHierarchy);

        private delegate void GetCanonicalNameDelegate(uint itemid, out string pbstrName);

        private delegate void GetPaneDelegate(ref Guid rguidPane, out IVsOutputWindowPane ppPane);

        /// <summary>
        /// Gets the mock service provider.
        /// </summary>
        internal static GlobalServiceProvider MockServiceProvider { get; private set; }

        /// <summary>
        /// Initializes resources or configurations needed before any tests in the assembly run.
        /// This method is executed once before all tests in the test assembly.
        /// </summary>
        /// <param name="context">Provides information about and functionality for the test run context.</param>
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            MockServiceProvider = new GlobalServiceProvider();
        }

        /// <summary>
        /// Cleans up resources or configurations after all tests in the assembly have run.
        /// This method is executed once after all tests in the test assembly have completed.
        /// </summary>
        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            MockServiceProvider.Dispose();
        }

        /// <summary>
        /// Initializes resources or configurations needed before each test runs.
        /// This method is executed once before every individual test in the class.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [TestInitialize]
        public async Task InitializeAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            MockServiceProvider.Reset();

            this.package = new PackageReferenceVersionToAttributeExtensionPackage();

            ServiceProvider globalProvider = ServiceProvider.GlobalProvider;
            OleServiceProvider oleServiceProvider = globalProvider.GetService<OleServiceProvider, OleServiceProvider>();
            ((IVsPackage)this.package).SetSite(oleServiceProvider);

            this.mockMultiItemSelect = new Mock<IVsMultiItemSelect>();
            this.mockHierarchy = new Mock<IVsHierarchy>();

            Mock<DTE2> mockDte = new();
            MockServiceProvider.AddService(typeof(DTE), mockDte.Object);

            Mock<Solution> mockDteSolution = new();
            mockDte
                .Setup(x => x.Solution)
                .Returns(mockDteSolution.Object);

            mockDteSolution
#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
                .Setup(x => x.FindProjectItem(It.IsAny<string>()))
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
                .Returns<ProjectItem>(null);

            // Create the mock for the Projects collection
            var mockProjects = new Mock<Projects>();

            // Set up the Projects collection to return an empty collection of projects
            mockProjects.As<IEnumerable<Project>>()
                .Setup(p => p.GetEnumerator())
                .Returns(new List<Project>().GetEnumerator());

            // Set up the Solution's Projects property to return the mocked Projects collection
            mockDteSolution
#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
                .SetupGet(x => x.Projects)
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
                .Returns(mockProjects.Object);

            Mock<IVsOutputWindow> mockOutputWindow = new();
            MockServiceProvider.AddService(typeof(SVsOutputWindow), mockOutputWindow.Object);

            Mock<IVsOutputWindowPane> mockOutputWindowPane = new();
            MockServiceProvider.AddService(typeof(SVsGeneralOutputWindowPane), mockOutputWindowPane.Object);

            mockOutputWindow
#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
                .Setup(x => x.GetPane(ref It.Ref<Guid>.IsAny, out It.Ref<IVsOutputWindowPane>.IsAny))
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
                .Callback(new GetPaneDelegate((ref Guid rguidPane, out IVsOutputWindowPane ppPane) =>
                {
                    ppPane = mockOutputWindowPane.Object;
                }))
                .Returns(VSConstants.S_OK);

            this.mockMonitorSelection = new Mock<IVsMonitorSelection>();
            this.mockMonitorSelection
#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
                .Setup(x => x.GetCurrentSelection(out It.Ref<IntPtr>.IsAny, out It.Ref<uint>.IsAny, out It.Ref<IVsMultiItemSelect>.IsAny, out It.Ref<IntPtr>.IsAny))
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
                .Callback(new GetCurrentSelectionDelegate((out IntPtr ppHier, out uint pitemid, out IVsMultiItemSelect ppMIS, out IntPtr ppSC) =>
                {
                    ppMIS = this.mockMultiItemSelect.Object;
                    ppSC = IntPtr.Zero;

                    if (this.mockHierarchy != null)
                    {
                        ppHier = Marshal.GetIUnknownForObject(this.mockHierarchy.Object);
                        if (this.mockMultiItemSelect == null)
                        {
                            pitemid = 0;
                        }
                        else
                        {
                            pitemid = VSConstants.VSITEMID_SELECTION;
                        }
                    }
                    else
                    {
                        ppHier = IntPtr.Zero;
                        pitemid = 0;
                    }
                }))
                .Returns(VSConstants.S_OK);

            MockServiceProvider.AddService(typeof(IVsMonitorSelection), this.mockMonitorSelection.Object);

            Mock<IComponentModel2> mockComponentModel = new();
            MockServiceProvider.AddService(typeof(SComponentModel), mockComponentModel.Object);

            Mock<IVsHierarchyItemManager> mockVsHierarchyItemManager = new();
            MockServiceProvider.AddService(typeof(IVsHierarchyItemManager), mockVsHierarchyItemManager.Object);

            var mockHierarchyItem = new Mock<IVsHierarchyItem>();
            var mockHierarchyItemIdentity = new Mock<IVsHierarchyItemIdentity>();
            mockHierarchyItem.Setup(x => x.HierarchyIdentity)
                .Returns(mockHierarchyItemIdentity.Object);

            mockHierarchyItemIdentity.Setup(x => x.Hierarchy)
                .Returns(this.mockHierarchy.Object);

            var mockProject = new Mock<IVsProject>();
            mockHierarchyItemIdentity.Setup(x => x.NestedHierarchy)
                .Returns(mockProject.As<IVsHierarchy>().Object);
            mockHierarchyItemIdentity.Setup(x => x.NestedItemID)
                .Returns(4294967294u);

            Mock<IVsSolution> mockSolution = new();
            MockServiceProvider.AddService(typeof(SVsSolution), mockSolution.Object);

            mockSolution
#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
                .Setup(s => s.GetVirtualProjectFlags(It.IsAny<IVsHierarchy>(), out It.Ref<uint>.IsAny))
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
                .Returns(VSConstants.E_FAIL);

            mockVsHierarchyItemManager.Setup(x => x.GetHierarchyItem(It.IsAny<IVsHierarchy>(), It.IsAny<uint>()))
                .Returns(mockHierarchyItem.Object);

            mockComponentModel.Setup(x => x.GetService<IVsHierarchyItemManager>())
                .Returns(mockVsHierarchyItemManager.Object);

            Mock<IAsyncServiceProvider> mockAsyncServiceProvider = new();
            Mock<IProfferAsyncService> mockPromotedServices = new();

            await ((IAsyncLoadablePackageInitialize)this.package).Initialize(
                mockAsyncServiceProvider.Object,
                mockPromotedServices.Object,
                mockPromotedServices.Object.GetServiceProgressCallback());

            IMenuCommandService commandService = (IMenuCommandService)await this.package.GetServiceAsync(typeof(IMenuCommandService));
            this.command = commandService.FindCommand(new CommandID(
                PackageGuids.guidPackageReferenceVersionToAttributeExtensionCmdSet,
                PackageIds.PackageReferenceVersionToAttributeCommand));
        }

        /// <summary>
        /// Verifies that the <c>ExecuteAsync</c> method completes successfully
        /// when no projects are selected in the solution.
        /// </summary>
        /// <returns>A task representing the asynchronous test operation.</returns>
        [TestMethod]
        public async Task ExecuteAsync_WithoutAnySelectedProjects_SucceedsAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            this.command.Invoke(null);
        }

        /// <summary>
        /// Verifies that the <c>ExecuteAsync</c> method completes successfully
        /// when one project is selected in the solution.
        /// </summary>
        /// <returns>A task representing the asynchronous test operation.</returns>
        [TestMethod]
        public async Task ExecuteAsync_WithOneSelectedProject_SucceedsAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            // create the project file
            using TempFile tempFile = new();
            using TempFile tempFileBak = new(tempFile.FilePath + ".bak");

            const string Contents = """
                <Project Sdk="Microsoft.NET.Sdk">
                    <PropertyGroup>
                        <TargetFramework>net8.0</TargetFramework>
                    </PropertyGroup>
                    <ItemGroup>
                        <PackageReference Include="Newtonsoft.Json">
                            <Version>13.0.3</Version>
                        </PackageReference>
                    </ItemGroup>
                </Project>
                """;

            File.WriteAllText(
                tempFile.FilePath,
                Contents);

            this.mockHierarchy
#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
                .Setup(h => h.GetCanonicalName(It.IsAny<uint>(), out It.Ref<string>.IsAny))
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
                .Callback(new GetCanonicalNameDelegate((uint id, out string fileName) =>
                {
                    fileName = tempFile.FilePath;
                }))
                .Returns(VSConstants.S_OK);

            this.mockMultiItemSelect
#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
                .Setup(x => x.GetSelectionInfo(out It.Ref<uint>.IsAny, out It.Ref<int>.IsAny))
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
                .Callback(new GetSelectionInfoDelegate((out uint pcItems, out int pfSingleHierarchy) =>
                {
                    // Simulate a single selected item
                    pcItems = 1;

                    // 1 indicates a single hierarchy selection
                    pfSingleHierarchy = 1;
                }))
                .Returns(VSConstants.S_OK);

            this.mockMultiItemSelect
#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
                .Setup(x => x.GetSelectedItems(It.IsAny<uint>(), It.IsAny<uint>(), It.IsAny<VSITEMSELECTION[]>()))
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
                .Callback((uint grfGSI, uint cItems, VSITEMSELECTION[] rgItemSel) =>
                {
                    // Ensure the array has at least one item
                    if (rgItemSel != null && rgItemSel.Length > 0)
                    {
                        rgItemSel[0] = new VSITEMSELECTION
                        {
                            // Mock hierarchy representing the selected project
                            pHier = this.mockHierarchy.Object,

                            // Use VSITEMID_ROOT to represent the root node of a project
                            itemid = VSConstants.VSITEMID_ROOT,
                        };
                    }
                })
                .Returns(VSConstants.S_OK);

            this.command.Invoke(null);

            Assert.AreEqual(
                """
                <Project Sdk="Microsoft.NET.Sdk">
                    <PropertyGroup>
                        <TargetFramework>net8.0</TargetFramework>
                    </PropertyGroup>
                    <ItemGroup>
                        <PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
                    </ItemGroup>
                </Project>
                """,
                File.ReadAllText(tempFile.FilePath));
        }
    }
}
