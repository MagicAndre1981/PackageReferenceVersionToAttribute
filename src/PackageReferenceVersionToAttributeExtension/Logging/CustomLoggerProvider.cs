// <copyright file="CustomLoggerProvider.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeExtension.Logging
{
    using System;
    using Microsoft.Extensions.Logging;
    using PackageReferenceVersionToAttributeExtension.Services;

    /// <summary>
    /// Custom logger provider.
    /// </summary>
    internal sealed class CustomLoggerProvider : ILoggerProvider, IDisposable
    {
        /// <inheritdoc/>
        public ILogger CreateLogger(string categoryName)
            => new OutputWindowLogger();

        /// <inheritdoc/>
        public void Dispose()
        {
        }
    }
}
