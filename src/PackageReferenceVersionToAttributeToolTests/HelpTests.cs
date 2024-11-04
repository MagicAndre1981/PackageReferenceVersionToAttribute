// <copyright file="UsageTests.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeToolTests
{
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PackageReferenceVersionToAttributeTool;
    using static PackageReferenceVersionToAttributeToolTests.ToolRunner.ToolRunner;

    /// <summary>
    /// Contains unit tests for verifying the expected behavior for the `--help` parameter of the tool.
    /// </summary>
    [TestClass]
    public class HelpTests
    {
        /// <summary>
        /// Verifies that a call to <see cref="Program.Main"/>
        /// with the `--help` parameter,
        /// returns a successful exit code and displays the command line usage on the console.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task Run_WithHelpParameter_DisplaysUsage()
        {
            // Act
            var result = await RunToolAsync("--help");

            // Assert
            Assert.AreEqual(0, result.ExitCode, result.OutputAndError);

            var expectedOutput = await File.ReadAllTextAsync("Usage.txt");
            Assert.AreEqual(expectedOutput, result.Output.Trim(), result.OutputAndError);
        }
    }
}