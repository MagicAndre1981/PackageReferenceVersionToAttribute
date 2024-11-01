// <copyright file="TempFile.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeExtensionTests
{
    using System;
    using System.IO;

    /// <summary>
    /// Temporary file.
    /// </summary>
    internal class TempFile : IDisposable
    {
        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="TempFile"/> class.
        /// </summary>
        public TempFile()
        {
            // Create a temporary file
            this.FilePath = Path.GetTempFileName();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TempFile"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public TempFile(string filePath)
        {
            this.FilePath = filePath;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="TempFile"/> class.
        /// </summary>
        ~TempFile()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Gets the file path.
        /// </summary>
        public string FilePath { get; private set; }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> if disposing; otherwise, <c>false</c>.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // Delete the temporary file
                    if (File.Exists(this.FilePath))
                    {
                        File.Delete(this.FilePath);
                    }
                }

                this.disposed = true;
            }
        }
    }
}