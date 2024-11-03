// <copyright file="ProjectConverterOptionsValidator.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttribute
{
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Validates options for the ProjectConverter, ensuring that configuration values
    /// in <see cref="ProjectConverterOptions"/> are set correctly.
    /// </summary>
    public class ProjectConverterOptionsValidator : IValidateOptions<ProjectConverterOptions>
    {
        /// <inheritdoc/>
        public ValidateOptionsResult Validate(string name, ProjectConverterOptions options)
        {
            if (options.DryRun && options.Backup)
            {
                return ValidateOptionsResult.Fail("Backup cannot be enabled when Dry Run is active.");
            }

            return ValidateOptionsResult.Success;
        }
    }
}
