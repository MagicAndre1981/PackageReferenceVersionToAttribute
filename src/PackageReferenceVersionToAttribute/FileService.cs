// <copyright file="FileService.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttribute
{
    using System.IO;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Provides support for operations on the file system.
    /// </summary>
    public class FileService : IFileService
    {
        private readonly ILogger<FileService> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public FileService(ILogger<FileService> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc/>
        public void RemoveReadOnlyAttribute(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            FileAttributes attributes = File.GetAttributes(filePath);
            if ((attributes & FileAttributes.ReadOnly) != FileAttributes.ReadOnly)
            {
                return;
            }

            // make the file read/write
            this.logger.LogDebug($"Removing read-only flag on file \"{filePath}\"...");

            attributes &= ~FileAttributes.ReadOnly;
            File.SetAttributes(filePath, attributes);
        }

        /// <inheritdoc/>
        public void BackupFile(string filePath)
        {
            string backupFilePath = $"{filePath}.bak";

            this.RemoveReadOnlyAttribute(backupFilePath);

            this.logger.LogDebug($"Copying \"{filePath}\" to \"{backupFilePath}\"...");

            File.Copy(filePath, backupFilePath, true);
        }
    }
}
