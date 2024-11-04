// <copyright file="ProgramCommand.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeTool
{
    using System.CommandLine;
    using System.CommandLine.NamingConventionBinder;

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
                description: "The file paths and patterns which match solution and project files to convert.")
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

            // validate
            var commandValidator = new ProgramCommandLineOptionsValidator(backupOption, forceOption, dryRunOption);
            this.AddValidator(commandValidator.Validate);

            // Set the handler for the command
            this.Handler = CommandHandler.Create<ProgramCommandLineOptions>(
                ProgramCommandHandler.HandleAsync);
        }
    }
}
