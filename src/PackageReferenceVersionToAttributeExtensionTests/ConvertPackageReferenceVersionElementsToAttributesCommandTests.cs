// <copyright file="ConvertPackageReferenceVersionElementsToAttributesCommandTests.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeExtensionTests
{
    using System.ComponentModel.Design;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.Sdk.TestFramework;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using PackageReferenceVersionToAttributeExtension;
    using PackageReferenceVersionToAttributeExtensionTests.Mocks;
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
        private MockVisualStudio mockVisualStudio;

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

            Mock<IAsyncServiceProvider> mockAsyncServiceProvider = new();
            Mock<IProfferAsyncService> mockPromotedServices = new();

            await ((IAsyncLoadablePackageInitialize)this.package).Initialize(
                mockAsyncServiceProvider.Object,
                mockPromotedServices.Object,
                mockPromotedServices.Object.GetServiceProgressCallback());

            this.mockVisualStudio = new MockVisualStudio(MockServiceProvider);

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
            // Arrange
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            // Act
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
            // Arrange
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            // create the project file
            const string Contents = """
                <Project Sdk="Microsoft.NET.Sdk">
                    <PropertyGroup>
                        <TargetFramework>net8.0</TargetFramework>
                    </PropertyGroup>
                    <ItemGroup>
                        <PackageReference Include="PackageA">
                            <Version>1.2.3</Version>
                        </PackageReference>
                    </ItemGroup>
                </Project>
                """;

            MockProject project = new MockProject(Contents);
            this.mockVisualStudio.AddProject(project);
            this.mockVisualStudio.AddSelection(project);

            // Act
            this.command.Invoke(null);

            // Assert
            Assert.AreEqual(
                """
                <Project Sdk="Microsoft.NET.Sdk">
                    <PropertyGroup>
                        <TargetFramework>net8.0</TargetFramework>
                    </PropertyGroup>
                    <ItemGroup>
                        <PackageReference Include="PackageA" Version="1.2.3" />
                    </ItemGroup>
                </Project>
                """,
                project.ReadFile());
        }

        /// <summary>
        /// Verifies that the <c>ExecuteAsync</c> method completes successfully
        /// when one project is selected in the solution.
        /// </summary>
        /// <returns>A task representing the asynchronous test operation.</returns>
        [TestMethod]
        public async Task ExecuteAsync_WithOneSelectedProjectWithMixedStyles_SucceedsAsync()
        {
            // Arrange
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            // create the project file
            const string Contents = """
                <Project Sdk="Microsoft.NET.Sdk">
                    <PropertyGroup>
                        <TargetFramework>net8.0</TargetFramework>
                    </PropertyGroup>
                    <ItemGroup>
                        <PackageReference Include="PackageA">
                          <Version>1.2.3</Version>
                          <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
                          <PrivateAssets>all</PrivateAssets>
                        </PackageReference>
                        <PackageReference Include="PackageB" Version="4.5.6" />
                    </ItemGroup>
                </Project>
                """;

            MockProject project = new MockProject(Contents);
            this.mockVisualStudio.AddProject(project);
            this.mockVisualStudio.AddSelection(project);

            // Act
            this.command.Invoke(null);

            // Assert
            Assert.AreEqual(
                """
                <Project Sdk="Microsoft.NET.Sdk">
                    <PropertyGroup>
                        <TargetFramework>net8.0</TargetFramework>
                    </PropertyGroup>
                    <ItemGroup>
                        <PackageReference Include="PackageA" Version="1.2.3">
                          
                          <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
                          <PrivateAssets>all</PrivateAssets>
                        </PackageReference>
                        <PackageReference Include="PackageB" Version="4.5.6" />
                    </ItemGroup>
                </Project>
                """,
                project.ReadFile());
        }

        /// <summary>
        /// Verifies that the <c>ExecuteAsync</c> method completes successfully
        /// when one project is selected in the solution.
        /// </summary>
        /// <returns>A task representing the asynchronous test operation.</returns>
        [TestMethod]
        public async Task ExecuteAsync_WithOneSelectedProjectWithConditionalElements_SucceedsAsync()
        {
            // Arrange
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            // create the project file
            const string Contents = """
                <Project Sdk="Microsoft.NET.Sdk">
                    <PropertyGroup>
                        <TargetFramework>net6.0;net8.0</TargetFramework>
                    </PropertyGroup>
                    <Choose>
                        <When Condition="'$(TargetFramework)'=='net6.0'">
                        <ItemGroup>
                            <PackageReference Include="PackageA">
                                <Version>1.2.3</Version>
                            </PackageReference>
                        </ItemGroup>
                        </When>
                        <When Condition="'$(TargetFramework)'=='net8.0'">
                        <ItemGroup>
                            <PackageReference Include="PackageB">
                                <Version>4.5.6</Version>
                            </PackageReference>
                        </ItemGroup>
                        </When>
                    </Choose>
                </Project>
                """;

            MockProject project = new MockProject(Contents);
            this.mockVisualStudio.AddProject(project);
            this.mockVisualStudio.AddSelection(project);

            // Act
            this.command.Invoke(null);

            // Assert
            Assert.AreEqual(
                """
                <Project Sdk="Microsoft.NET.Sdk">
                    <PropertyGroup>
                        <TargetFramework>net6.0;net8.0</TargetFramework>
                    </PropertyGroup>
                    <Choose>
                        <When Condition="'$(TargetFramework)'=='net6.0'">
                        <ItemGroup>
                            <PackageReference Include="PackageA" Version="1.2.3" />
                        </ItemGroup>
                        </When>
                        <When Condition="'$(TargetFramework)'=='net8.0'">
                        <ItemGroup>
                            <PackageReference Include="PackageB" Version="4.5.6" />
                        </ItemGroup>
                        </When>
                    </Choose>
                </Project>
                """,
                project.ReadFile());
        }
    }
}
