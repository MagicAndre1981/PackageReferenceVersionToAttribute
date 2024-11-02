// <copyright file="Program.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeTool
{
    using System;
    using System.CommandLine;
    using System.CommandLine.NamingConventionBinder;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using PackageReferenceVersionToAttribute;

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
            var pathArgument = new Argument<string[]>(
                name: "inputs",
                description: "The project files or wildcard patterns to convert.")
            {
                Arity = ArgumentArity.OneOrMore,
            };

            var backupOption = new Option<bool>(
                aliases: ["--backup", "-b"],
                description: "Create a backup of the project files.");

            var forceOption = new Option<bool>(
                aliases: ["--force", "-f"],
                description: "Force conversion even if already configured.");

            var dryRunOption = new Option<bool>(
                aliases: ["--dry-run", "-d"],
                "Preview changes without making any modifications.");

            var rootCommand = new RootCommand
            {
                pathArgument,
                backupOption,
                forceOption,
                dryRunOption,
            };

            // Set the handler for the command
            rootCommand.Handler = CommandHandler.Create(
                async (string[] inputs, bool backup, bool force, bool dryRun) =>
                {
                    // Bind command line arguments
                    var options = new ProjectConverterOptions
                    {
                        Backup = backup,
                        Force = force,
                        DryRun = dryRun,
                    };

                    foreach (var input in inputs)
                    {
                        await ConvertPackageReferencesAsync(input, options);
                    }
                });

            // Invoke the command line parser
            return await rootCommand.InvokeAsync(args);
        }

        private static async Task ConvertPackageReferencesAsync(
            string input, ProjectConverterOptions options)
        {
            FilePatternMatcher filePatternMatcher = new();
            List<string> matchingFiles = filePatternMatcher.GetMatchingFiles(input);
            if (matchingFiles.Count == 0)
            {
                Console.WriteLine($"No matching files found for pattern: {input}");
                return;
            }

            if (options.Backup)
            {
                Console.WriteLine("Backup option is enabled.");
            }

            if (options.Force)
            {
                Console.WriteLine("Force option is enabled.");
            }

            if (options.DryRun)
            {
                Console.WriteLine("Dry run mode is enabled. No changes will be made.");
            }

            using var serviceProvider = new ServiceCollection()
                .AddSingleton(Options.Create(options))
                .AddLogging(configure => configure.AddConsole())
                .AddSingleton<ProjectConverter>()
                .AddSingleton<IFileService, FileService>()
                .AddSingleton<ISourceControlService, NullSourceControlService>()
                .BuildServiceProvider();

            var projectConverter = serviceProvider.GetRequiredService<ProjectConverter>();

            await projectConverter.ConvertAsync(matchingFiles);
        }
    }
}