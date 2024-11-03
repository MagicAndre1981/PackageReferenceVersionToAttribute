// <copyright file="Program.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeTool
{
    using System.CommandLine;
    using System.Threading.Tasks;

    /// <summary>
    /// The entry point for the application.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Entry point for the application that processes command-line arguments asynchronously.
        /// </summary>
        /// <param name="args">Command-line arguments passed to the application.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the exit code.
        /// </returns>
        internal static async Task<int> Main(string[] args)
        {
            ProgramCommand command = new ProgramCommand();

            return await command.InvokeAsync(args);
        }
    }
}