// <copyright file="ToolRunner.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeToolTests.ToolRunner
{
    using System.Diagnostics;
    using System.Text;

    /// <summary>
    /// Provides functionality to run the PackageReferenceVersionToAttributeTool executable with specified arguments and capture the results.
    /// </summary>
    internal class ToolRunner
    {
        private const string ToolExecutable = "PackageReferenceVersionToAttributeTool.exe";

        /// <summary>
        /// Executes the tool asynchronously with the specified command-line arguments.
        /// </summary>
        /// <param name="arguments">The arguments to pass to the tool. Can be null if no arguments are required.</param>
        /// <returns>A <see cref="RunResult"/> containing the result from running the tool.</returns>
        public static async Task<RunResult> RunToolAsync(string arguments = null)
        {
            Console.WriteLine($"Running tool with arguments: {arguments}");
            var processStartInfo = new ProcessStartInfo
            {
                FileName = ToolExecutable,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false, // required for redirection
                CreateNoWindow = true, // do not create a window
            };

            using var process = new Process
            {
                StartInfo = processStartInfo,
            };

            var outputBuilder = new StringBuilder();
            var errorBuilder = new StringBuilder();

            process.OutputDataReceived += (sender, e) =>
            {
                if (e.Data != null)
                {
                    outputBuilder.AppendLine(e.Data);
                }
            };

            process.ErrorDataReceived += (sender, e) =>
            {
                if (e.Data != null)
                {
                    errorBuilder.AppendLine(e.Data);
                }
            };

            process.Start();

            // Start async reading
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            await Task.Run(() => process.WaitForExit()); // Wait for exit asynchronously

            return new RunResult
            {
                ExitCode = process.ExitCode,
                Output = outputBuilder.ToString(),
                Error = errorBuilder.ToString(),
            };
        }
    }
}
