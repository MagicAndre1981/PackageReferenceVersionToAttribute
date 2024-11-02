// <copyright file="IFileService.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttribute
{
    /// <summary>
    /// Provides file-related operations, such as backing up files and modifying attributes.
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Backs up the specified file.
        /// </summary>
        /// <param name="filePath">The path of the file.</param>
        void BackupFile(string filePath);

        /// <summary>
        /// Removes the read-only attribute on the specified file.
        /// </summary>
        /// <param name="filePath">The path of the file.</param>
        void RemoveReadOnlyAttribute(string filePath);
    }
}
