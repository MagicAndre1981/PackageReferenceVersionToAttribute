// <copyright file="ConvertPackageReferenceVersionElementsToAttributesCommandTests.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeExtensionTests
{
    using System;
    using System.ComponentModel.Design;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.Sdk.TestFramework;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using PackageReferenceVersionToAttributeExtension;
    using PackageReferenceVersionToAttributeExtensionTests.Mocks;
    using PackageReferenceVersionToAttributeExtensionTests.Setup;
    using IAsyncServiceProvider = Microsoft.VisualStudio.Shell.Interop.COMAsyncServiceProvider.IAsyncServiceProvider;
    using OleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;
    using ServiceProvider = Microsoft.VisualStudio.Shell.ServiceProvider;

    /// <summary>
    /// Convert PackageReference version elements to attributes command tests.
    /// </summary>
    [TestClass]
    public class ConvertPackageReferenceVersionElementsToAttributesCommandTests
    {
        private PackageReferenceVersionToAttributeExtensionPackage package;
        private MenuCommand command;
        private MockVisualStudio mockVisualStudio;
        private DisposableServiceProvider serviceProvider;
        private ILoggerFactory loggerFactory;

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
#pragma warning disable IDE0060 // Remove unused parameter; context is required by the MSTest framework
        public static void AssemblyInitialize(TestContext context)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            MockServiceProvider?.Dispose();
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
        /// Runs before each test.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [TestInitialize]
        public async Task TestInitializeAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            MockServiceProvider.Reset();
            this.serviceProvider = ServiceProviderFactory.CreateServiceProvider();

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

            this.mockVisualStudio = new MockVisualStudio(MockServiceProvider, this.serviceProvider);

            IMenuCommandService commandService = (IMenuCommandService)await this.package.GetServiceAsync(typeof(IMenuCommandService));
            Assert.IsNotNull(commandService);

            this.command = commandService.FindCommand(new CommandID(
                PackageGuids.guidPackageReferenceVersionToAttributeExtensionCmdSet,
                PackageIds.PackageReferenceVersionToAttributeCommand));
            Assert.IsNotNull(this.command);

            this.loggerFactory = this.serviceProvider.GetService<ILoggerFactory>();
        }

        /// <summary>
        /// Runs after each test method.
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            this.mockVisualStudio.Dispose();
            (this.serviceProvider as IDisposable)?.Dispose();
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

            using MockProject project = new(this.loggerFactory)
            {
                Name = "ProjectA",
                Contents = """
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
                    """,
            };
            this.mockVisualStudio.AddProjects(project);
            this.mockVisualStudio.AddSelections(project);

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
                project.Contents);
        }

        /// <summary>
        /// Verifies that the <c>ExecuteAsync</c> method completes successfully
        /// when one project is selected in the solution,
        /// with mixed PackageReference Version styles.
        /// </summary>
        /// <returns>A task representing the asynchronous test operation.</returns>
        [TestMethod]
        public async Task ExecuteAsync_WithOneSelectedProjectWithMixedStyles_SucceedsAsync()
        {
            // Arrange
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            using MockProject project = new(this.loggerFactory)
            {
                Name = "ProjectA",
                Contents = """
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
                    """,
            };
            this.mockVisualStudio.AddProjects(project);
            this.mockVisualStudio.AddSelections(project);

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
                project.Contents);
        }

        /// <summary>
        /// Verifies that the <c>ExecuteAsync</c> method completes successfully
        /// when one project is selected in the solution,
        /// with PackageReferences within nested conditional elements.
        /// </summary>
        /// <returns>A task representing the asynchronous test operation.</returns>
        [TestMethod]
        public async Task ExecuteAsync_WithOneSelectedProjectWithConditionalElements_SucceedsAsync()
        {
            // Arrange
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            using MockProject project = new(this.loggerFactory)
            {
                Name = "ProjectA",
                Contents = """
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
                    """,
            };
            this.mockVisualStudio.AddProjects(project);
            this.mockVisualStudio.AddSelections(project);

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
                project.Contents);
        }

        /// <summary>
        /// Verifies that the <c>ExecuteAsync</c> method completes successfully
        /// when one project is selected in the solution,
        /// which contains an XML namespace.
        /// </summary>
        /// <returns>A task representing the asynchronous test operation.</returns>
        [TestMethod]
        public async Task ExecuteAsync_WithOneSelectedProjectWithAnXmlNamespace_SucceedsAsync()
        {
            // Arrange
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            using MockProject project = new(this.loggerFactory)
            {
                Name = "ProjectA",
                Contents = """
                    <Project xmlns="http://schemas.microsoft.com/developer/msbuild/2003" Sdk="Microsoft.NET.Sdk.Uwp">

                      <PropertyGroup>
                        <TargetFramework>uap10.0</TargetFramework>
                        <RootNamespace>ExampleUWPProject</RootNamespace>
                        <AssemblyName>ExampleUWPProject</AssemblyName>
                      </PropertyGroup>

                      <ItemGroup>
                        <PackageReference Include="Microsoft.NETCore.UniversalWindowsPlatform">
                            <Version>6.2.10</Version>
                        </PackageReference>
                        <PackageReference Include="Newtonsoft.Json">
                            <Version>13.0.1</Version>
                        </PackageReference>
                        <PackageReference Include="Microsoft.Toolkit.Uwp.UI.Controls">
                            <Version>6.1.2</Version>
                        </PackageReference>
                        <PackageReference Include="Microsoft.Extensions.Logging">
                            <Version>5.0.0</Version>
                        </PackageReference>
                      </ItemGroup>

                    </Project>
                    """,
            };
            this.mockVisualStudio.AddProjects(project);
            this.mockVisualStudio.AddSelections(project);

            // Act
            this.command.Invoke(null);

            // Assert
            Assert.AreEqual(
                """
                <Project xmlns="http://schemas.microsoft.com/developer/msbuild/2003" Sdk="Microsoft.NET.Sdk.Uwp">

                  <PropertyGroup>
                    <TargetFramework>uap10.0</TargetFramework>
                    <RootNamespace>ExampleUWPProject</RootNamespace>
                    <AssemblyName>ExampleUWPProject</AssemblyName>
                  </PropertyGroup>

                  <ItemGroup>
                    <PackageReference Include="Microsoft.NETCore.UniversalWindowsPlatform" Version="6.2.10" />
                    <PackageReference Include="Newtonsoft.Json" Version="13.0.1" />
                    <PackageReference Include="Microsoft.Toolkit.Uwp.UI.Controls" Version="6.1.2" />
                    <PackageReference Include="Microsoft.Extensions.Logging" Version="5.0.0" />
                  </ItemGroup>

                </Project>
                """,
                project.Contents);
        }

        /// <summary>
        /// Verifies that the <c>ExecuteAsync</c> method completes successfully
        /// when two projects are selected in the solution.
        /// </summary>
        /// <returns>A task representing the asynchronous test operation.</returns>
        [TestMethod]
        public async Task ExecuteAsync_WithTwoSelectedProjects_SucceedsAsync()
        {
            // Arrange
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            using MockProject project1 = new(this.loggerFactory)
            {
                Id = 1,
                Name = "ProjectA",
                Contents = """
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
                    """,
            };

            using MockProject project2 = new(this.loggerFactory)
            {
                Id = 2,
                Name = "ProjectB",
                Contents = """
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
                    """,
            };

            this.mockVisualStudio.AddProjects(project1, project2);
            this.mockVisualStudio.AddSelections(project1, project2);

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
                project1.Contents);
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
                project2.Contents);
        }
    }
}
