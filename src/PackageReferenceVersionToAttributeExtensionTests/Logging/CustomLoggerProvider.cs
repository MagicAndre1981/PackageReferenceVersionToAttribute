// <copyright file="CustomLoggerProvider.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeExtensionTests.Logging
{
    using System;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Custom logger provider.
    /// </summary>
    internal sealed class CustomLoggerProvider : ILoggerProvider, IDisposable
    {
        /// <inheritdoc/>
        public ILogger CreateLogger(string categoryName)
            => new CustomConsoleLogger(categoryName);

        /// <inheritdoc/>
        public void Dispose()
        {
        }
    }
}
