// <copyright file="CustomLoggerProvider.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeExtension.Logging
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using PackageReferenceVersionToAttributeExtension.Services;

    /// <summary>
    /// Custom logger provider.
    /// </summary>
    internal sealed class CustomLoggerProvider : ILoggerProvider, IDisposable
    {
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomLoggerProvider"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public CustomLoggerProvider(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        /// <inheritdoc/>
        public ILogger CreateLogger(string categoryName)
            => this.serviceProvider.GetRequiredService<OutputWindowLogger>();

        /// <inheritdoc/>
        public void Dispose()
        {
        }
    }
}
