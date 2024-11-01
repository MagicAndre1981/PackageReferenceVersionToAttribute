// <copyright file="DisposableServiceProvider.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeExtensionTests.Setup
{
    using System;

    /// <summary>
    /// Disposable service provider.
    /// </summary>
    internal sealed class DisposableServiceProvider : IDisposable
    {
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableServiceProvider"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public DisposableServiceProvider(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Retrieves an instance of the specified service type from the service provider.
        /// </summary>
        /// <typeparam name="T">The type of the service to retrieve.</typeparam>
        /// <returns>An instance of the specified service type.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the service is not found.</exception>
        public T GetService<T>() => (T)this.serviceProvider.GetService(typeof(T));

        /// <summary>
        /// Retrieves an instance of the specified service type from the service provider,
        /// ensuring that the service is registered and not null.
        /// </summary>
        /// <typeparam name="T">The type of the service to retrieve.</typeparam>
        /// <returns>An instance of the specified service type.</returns>
        public T GetRequiredService<T>()
        {
            var service = this.serviceProvider.GetService(typeof(T))
                ?? throw new InvalidOperationException($"Service of type {typeof(T).Name} is not registered.");

            return (T)service;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            // Dispose of the service provider if it's disposable
            if (this.serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
