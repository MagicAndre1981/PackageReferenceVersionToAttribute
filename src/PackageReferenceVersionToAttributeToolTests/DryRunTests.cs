// <copyright file="DryRunTests.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeToolTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PackageReferenceVersionToAttributeTool;
    using PackageReferenceVersionToAttributeToolTests.FileSystem;
    using static PackageReferenceVersionToAttributeToolTests.ToolRunner.ToolRunner;

    /// <summary>
    /// Contains unit tests for verifying the expected behavior for the `--dry-run` parameter of the tool.
    /// </summary>
    [TestClass]
    public class DryRunTests
    {
        /// <summary>
        /// Verifies that a call to <see cref="Program.Main"/>
        /// with the `--dry-run` parameter,
        /// does not convert the project file.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task Run_WithDryRunParameter_DoesNotConvertProjectFile()
        {
            // Arrange
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
            using var projectA = new TempFile(
                "ProjectA.csproj",
                Contents);

            using var testDirectory = new TempDir
            {
                projectA,
            };

            // Act
            var result = await RunToolAsync($"\"{projectA.Path}\" --dry-run");

            // Assert
            Assert.AreEqual(0, result.ExitCode, result.OutputAndError);
            Assert.AreEqual(
                $"""
                Dry run mode is enabled. No changes will be made.
                info: PackageReferenceVersionToAttribute.ProjectConverter[0]
                      Converting PackageReference Version child elements to attributes in the project file "{projectA.Path}"...
                info: PackageReferenceVersionToAttribute.ProjectConverter[0]
                      <Project Sdk="Microsoft.NET.Sdk">
                          <PropertyGroup>
                              <TargetFramework>net8.0</TargetFramework>
                          </PropertyGroup>
                          <ItemGroup>
                              <PackageReference Include="PackageA" Version="1.2.3" />
                          </ItemGroup>
                      </Project>
                """,
                result.Output.Trim(),
                result.OutputAndError);
            Assert.AreEqual(string.Empty, result.Error.Trim());

            Assert.IsFalse(File.Exists($"{projectA.Path}.bak"));
        }
    }
}
