// <copyright file="ProjectConverterOptions.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttribute
{
    /// <summary>
    /// Represents the options for configuring the project converter behavior.
    /// </summary>
    public class ProjectConverterOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether to back up project files before conversion.
        /// </summary>
        public bool Backup { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to force conversion of project files,
        /// even if they are marked as read-only.
        /// </summary>
        public bool Force { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to perform a dry run,
        /// simulating the conversion without making any changes.
        /// </summary>
        public bool DryRun { get; set; }
    }
}