// <copyright file="ProgramCommandLineOptions.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeTool
{
    using System.Collections.Generic;
    using PackageReferenceVersionToAttribute;

    /// <summary>
    /// Program command line options.
    /// </summary>
    internal class ProgramCommandLineOptions : ProjectConverterOptions
    {
        /// <summary>
        /// Gets or sets the project files or wildcard patterns to convert.
        /// </summary>
        public IEnumerable<string> Inputs { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to display the version information of the application.
        /// </summary>
        public bool Version { get; set; }
    }
}
