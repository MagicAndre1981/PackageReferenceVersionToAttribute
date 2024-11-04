// <copyright file="ProgramCommandLineOptionsValidator.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeTool
{
    using System.CommandLine;
    using System.CommandLine.Parsing;
    using PackageReferenceVersionToAttribute;

    /// <summary>
    /// Validates command-line options for the program.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ProgramCommandLineOptionsValidator"/> class.
    /// </remarks>
    /// <param name="backupOption">The backup option.</param>
    /// <param name="forceOption">The force option.</param>
    /// <param name="dryRunOption">The dry run option.</param>
    internal class ProgramCommandLineOptionsValidator(
        Option<bool> backupOption,
        Option<bool> forceOption,
        Option<bool> dryRunOption)
    {
        private readonly Option<bool> backupOption = backupOption;
        private readonly Option<bool> forceOption = forceOption;
        private readonly Option<bool> dryRunOption = dryRunOption;

        /// <summary>
        /// Validates the specified <see cref="CommandResult"/>.
        /// </summary>
        /// <param name="result">The <see cref="CommandResult"/> containing the parsed command-line options.</param>
        public void Validate(CommandResult result)
        {
            // Extract option values
            bool backup = result.GetValueForOption(this.backupOption);
            bool force = result.GetValueForOption(this.forceOption);
            bool dryRun = result.GetValueForOption(this.dryRunOption);

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
        }
    }
}
