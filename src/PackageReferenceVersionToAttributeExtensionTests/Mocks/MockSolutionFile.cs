// <copyright file="MockSolutionFile.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeExtensionTests.Mocks
{
    using System;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Mock solution file.
    /// </summary>
    internal sealed class MockSolutionFile : MockHierarchy, IDisposable
    {
        private readonly TempFile tempFile = new();
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="MockSolutionFile"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        public MockSolutionFile(ILoggerFactory loggerFactory)
            : base(loggerFactory.CreateLogger<MockHierarchy>())
        {
            this.Name = this.tempFile.FilePath;
            this.Id = 0;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.tempFile.Dispose();

            this.disposed = true;
        }
    }
}