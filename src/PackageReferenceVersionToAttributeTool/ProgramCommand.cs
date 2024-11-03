// <copyright file="ProgramCommand.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeTool
{
    using System;
    using System.Collections.Generic;
    using System.CommandLine;
    using System.CommandLine.NamingConventionBinder;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using PackageReferenceVersionToAttribute;

    /// <summary>
    /// Program command.
    /// </summary>
    internal class ProgramCommand : RootCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProgramCommand"/> class.
        /// </summary>
        public ProgramCommand()
            : base("Converts PackageReference Version child elements to attributes in C# projects.")
        {
            var inputsArgument = new Argument<string[]>(
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
                description: "Force conversion even if the project files are read-only.");

            var dryRunOption = new Option<bool>(
                aliases: ["--dry-run", "-d"],
                description: "Preview changes without making any modifications.");

            var versionOption = new Option<bool>("--version", "Display the version information.");

            this.Add(inputsArgument);
            this.Add(backupOption);
            this.Add(forceOption);
            this.Add(dryRunOption);

            // Add validation
            this.AddValidator(result =>
            {
                bool backup = result.GetValueForOption(backupOption);
                bool force = result.GetValueForOption(forceOption);
                bool dryRun = result.GetValueForOption(dryRunOption);

                var options = new ProjectConverterOptions
                {
                    Backup = backup,
                    Force = force,
                    DryRun = dryRun,
                };

                var validator = new ProjectConverterOptionsValidator();
                var validationResult = validator.Validate(nameof(ProjectConverterOptions), options);
                if (validationResult.Failed)
                {
                    result.ErrorMessage = validationResult.FailureMessage;
                }
            });

            // Set the handler for the command
            this.Handler = CommandHandler.Create<ProgramCommandLineOptions>(async (options) =>
            {
                if (options.Version)
                {
                    var version = Assembly.GetExecutingAssembly().GetName().Version;
                    Console.WriteLine($"Version: {version}");
                    return;
                }

                foreach (var input in options.Inputs)
                {
                    await ConvertPackageReferencesAsync(input, options);
                }
            });
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
                .AddSingleton(Microsoft.Extensions.Options.Options.Create(options))
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
