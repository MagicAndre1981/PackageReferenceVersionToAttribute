// <copyright file="RunResult.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeToolTests.ToolRunner
{
    /// <summary>
    /// Represents the result of running a process.
    /// </summary>
    internal class RunResult
    {
        /// <summary>
        /// Gets or sets the exit code returned by the process.
        /// </summary>
        public int ExitCode { get; set; }

        /// <summary>
        /// Gets or sets the standard output of the process.
        /// </summary>
        public string Output { get; set; }

        /// <summary>
        /// Gets or sets the error output of the process.
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Gets a combined output of standard and error outputs.
        /// </summary>
        public string OutputAndError
        {
            get
            {
                var outputLabel = string.IsNullOrEmpty(this.Output) ? string.Empty : $"Output:\n{this.Output}\n";
                var errorLabel = string.IsNullOrEmpty(this.Error) ? string.Empty : $"Error:\n{this.Error}\n";
                return outputLabel + errorLabel;
            }
        }
    }
}