// <copyright file="BackupTests.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeToolTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PackageReferenceVersionToAttributeTool;
    using PackageReferenceVersionToAttributeToolTests.FileSystem;
    using static PackageReferenceVersionToAttributeToolTests.ToolRunner.ToolRunner;

    /// <summary>
    /// Contains unit tests for verifying the expected behavior for the `--backup` parameter of the tool.
    /// </summary>
    [TestClass]
    public class BackupTests
    {
        /// <summary>
        /// Verifies that a call to <see cref="Program.Main"/>
        /// without the `--backup` parameter,
        /// does not backup the project file.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task Run_WithoutBackupParameter_DoesNotBackupProjectFile()
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
            var result = await RunToolAsync($"\"{projectA.Path}\"");

            // Assert
            Assert.AreEqual(0, result.ExitCode, result.OutputAndError);
            Assert.AreEqual(
                $"""
                info: PackageReferenceVersionToAttribute.ProjectConverter[0]
                      Converting PackageReference Version child elements to attributes in the project file "{projectA.Path}"...
                """,
                result.Output.Trim(),
                result.OutputAndError);
            Assert.AreEqual(string.Empty, result.Error.Trim());

            Assert.IsFalse(File.Exists($"{projectA.Path}.bak"));
        }

        /// <summary>
        /// Verifies that a call to <see cref="Program.Main"/>
        /// with the `--backup` parameter,
        /// backs up the project file.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task Run_WithBackupParameter_BacksUpProjectFile()
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
            var result = await RunToolAsync($"\"{projectA.Path}\" --backup");

            // Assert
            Assert.AreEqual(0, result.ExitCode, result.OutputAndError);
            Assert.AreEqual(
                $"""
                Backup option is enabled.
                info: PackageReferenceVersionToAttribute.ProjectConverter[0]
                      Converting PackageReference Version child elements to attributes in the project file "{projectA.Path}"...
                """,
                result.Output.Trim(),
                result.OutputAndError);
            Assert.AreEqual(string.Empty, result.Error.Trim());

            string backupFilePath = $"{projectA.Path}.bak";
            Assert.IsTrue(File.Exists(backupFilePath));
            Assert.AreEqual(Contents, File.ReadAllText(backupFilePath));
        }
    }
}
