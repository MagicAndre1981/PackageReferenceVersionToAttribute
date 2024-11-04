// <copyright file="ProgramCommandHandler.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeTool
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using PackageReferenceVersionToAttribute;
    using SlnParser;
    using SlnParser.Contracts;

    /// <summary>
    /// Program command handler.
    /// </summary>
    internal class ProgramCommandHandler
    {
        /// <summary>
        /// Executes the command logic asynchronously based on the provided command line options.
        /// </summary>
        /// <param name="options">An instance of <see cref="ProgramCommandLineOptions"/> containing the parsed options from the command line.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task HandleAsync(ProgramCommandLineOptions options)
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
        }

        private static async Task ConvertPackageReferencesAsync(
            string input, ProjectConverterOptions options)
        {
            FilePatternMatcher filePatternMatcher = new();

            // get matching csproj and sln files
            List<string> matchingFiles = filePatternMatcher.GetMatchingFiles(input)
                .Where(x => x.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase)
                    || x.EndsWith(".sln", StringComparison.OrdinalIgnoreCase))
                .ToList();

            // parse sln files to get only csproj files
            List<string> projectFiles = GetCsprojFiles(matchingFiles);
            if (projectFiles.Count == 0)
            {
                Console.WriteLine($"No matching project files found for pattern: {input}");
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

            await projectConverter.ConvertAsync(projectFiles);
        }

        private static List<string> GetCsprojFiles(List<string> files)
        {
            var csprojFiles = new List<string>();

            foreach (var file in files)
            {
                if (file.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase))
                {
                    csprojFiles.Add(file);
                }
                else if (file.EndsWith(".sln", StringComparison.OrdinalIgnoreCase))
                {
                    csprojFiles.AddRange(GetCsprojFiles(file));
                }
            }

            return csprojFiles;
        }

        private static IEnumerable<string> GetCsprojFiles(string solutionFilePath)
        {
            SolutionParser solutionParser = new SolutionParser();
            ISolution solution = solutionParser.Parse(solutionFilePath);

            return solution.AllProjects
                .OfType<SolutionProject>()
                .Where(x => x.File.FullName.EndsWith(".csproj"))
                .Select(x => x.File.FullName);
        }
    }
}
