// <copyright file="NullFileService.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeTool
{
    using PackageReferenceVersionToAttribute;

    /// <summary>
    /// A no-op implementation of the <see cref="IFileService"/> interface.
    /// </summary>
    internal class NullFileService : IFileService
    {
        /// <inheritdoc/>
        public void BackupFile(string path)
        {
        }

        /// <inheritdoc/>
        public void RemoveReadOnlyAttribute(string filePath)
        {
        }
    }
}
