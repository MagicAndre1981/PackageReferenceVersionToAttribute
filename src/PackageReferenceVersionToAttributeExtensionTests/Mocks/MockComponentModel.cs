// <copyright file="MockComponentModel.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeExtensionTests.Mocks
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.ComponentModel.Composition.Primitives;
    using Microsoft.VisualStudio.ComponentModelHost;

    /// <summary>
    /// Mock component model.
    /// </summary>
    internal class MockComponentModel : IComponentModel2, SComponentModel
    {
        private readonly Dictionary<Type, object> services = new();

        /// <inheritdoc/>
        public CompositionScopeDefinition DefaultScopedCatalog => throw new NotImplementedException();

        /// <inheritdoc/>
        public ComposablePartCatalog DefaultCatalog => throw new NotImplementedException();

        /// <inheritdoc/>
        public ExportProvider DefaultExportProvider => throw new NotImplementedException();

        /// <inheritdoc/>
        public ICompositionService DefaultCompositionService => throw new NotImplementedException();

        /// <inheritdoc/>
        public ComposablePartCatalog GetCatalog(string catalogName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IEnumerable<T> GetExtensions<T>()
            where T : class
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Registers a service instance of type <typeparamref name="T"/> to be returned by <see cref="GetService{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the service to register.</typeparam>
        /// <param name="service">The instance of the service to register.</param>
        public void AddService<T>(T service)
            where T : class
        {
            this.services[typeof(T)] = service;
        }

        /// <inheritdoc/>
        public T GetService<T>()
            where T : class
        {
            // Try to retrieve the service, or return null if not registered
            if (this.services.TryGetValue(typeof(T), out var service))
            {
                return service as T;
            }

            throw new NotImplementedException($"Service for type {typeof(T).Name} not implemented.");
        }
    }
}