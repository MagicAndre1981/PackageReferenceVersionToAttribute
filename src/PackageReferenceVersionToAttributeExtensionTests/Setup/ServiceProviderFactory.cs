// <copyright file="ServiceProviderFactory.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeExtensionTests.Setup
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using PackageReferenceVersionToAttributeExtensionTests.Logging;
    using PackageReferenceVersionToAttributeExtensionTests.Mocks;

    /// <summary>
    /// Service provider factory.
    /// </summary>
    internal static class ServiceProviderFactory
    {
        /// <summary>
        /// Creates and configures an <see cref="IServiceProvider"/> for dependency injection in tests.
        /// This includes setting up logging and registering various mock services used for testing.
        /// </summary>
        /// <returns>An instance of <see cref="DisposableServiceProvider"/> configured with the necessary services.</returns>
        public static DisposableServiceProvider CreateServiceProvider()
        {
            var serviceCollection = new ServiceCollection();

            // logging
            serviceCollection.AddSingleton<CustomLoggerProvider>();
            serviceCollection.AddLogging(configure =>
            {
                configure.ClearProviders();
                configure.Services.AddSingleton<ILoggerProvider, CustomLoggerProvider>();
                configure.SetMinimumLevel(LogLevel.Trace);
            });

            // Register services
            RegisterServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            return new DisposableServiceProvider(serviceProvider);
        }

        private static void RegisterServices(ServiceCollection services)
        {
            services.AddSingleton<MockMultiItemSelect>();
            services.AddSingleton<MockDevelopmentToolsEnvironment>();
            services.AddSingleton<MockDteSolution>();
            services.AddSingleton<MockProjects>();
            services.AddSingleton<MockOutputWindowPane>();
            services.AddSingleton<MockOutputWindow>();
            services.AddSingleton<MockMonitorSelection>();
            services.AddSingleton<MockComponentModel>();
            services.AddSingleton<MockHierarchy>();
            services.AddSingleton<MockHierarchyItem>();
            services.AddSingleton<MockHierarchyItemIdentity>();
            services.AddSingleton<MockHierarchyItemManager>();
            services.AddSingleton<MockSolution>();
            services.AddSingleton<MockStatusbar>();
        }
    }
}
