// <copyright file="NullSourceControlService.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeTool
{
    using PackageReferenceVersionToAttribute;

    /// <summary>
    /// A no-op implementation of the <see cref="ISourceControlService"/> interface.
    /// </summary>
    internal class NullSourceControlService : ISourceControlService
    {
        /// <inheritdoc/>
        public Task CheckOutFileAsync(string path)
        {
            return Task.CompletedTask;
        }
    }
}
