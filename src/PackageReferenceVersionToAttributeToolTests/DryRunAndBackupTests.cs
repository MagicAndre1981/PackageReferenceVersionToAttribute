// <copyright file="DryRunAndBackupTests.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeToolTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PackageReferenceVersionToAttributeTool;
    using PackageReferenceVersionToAttributeToolTests.FileSystem;
    using static PackageReferenceVersionToAttributeToolTests.ToolRunner.ToolRunner;

    /// <summary>
    /// Contains unit tests for verifying the expected behavior
    /// for the `--dry-run` and `--backup` parameters of the tool,
    /// when used together.
    /// </summary>
    [TestClass]
    public class DryRunAndBackupTests
    {
        /// <summary>
        /// Verifies that a call to <see cref="Program.Main"/>
        /// with the `--dry-run` and `--backup` parameters,
        /// fails with a validation error.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task Run_WithDryRunAndBackupParameters_FailsWithValidationError()
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

            var usage = await File.ReadAllTextAsync("Usage.txt");

            // Act
            var result = await RunToolAsync($"\"{projectA.Path}\" --dry-run --backup");

            // Assert
            Assert.AreEqual(1, result.ExitCode, result.OutputAndError);
            Assert.AreEqual(usage, result.Output.Trim(), result.OutputAndError);
            Assert.AreEqual(
                "Backup cannot be enabled when Dry Run is active.",
                result.Error.Trim());

            Assert.IsFalse(File.Exists($"{projectA.Path}.bak"));
        }
    }
}
