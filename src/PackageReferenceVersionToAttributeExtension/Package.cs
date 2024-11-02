// <copyright file="Package.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeExtension
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Threading;
    using Community.VisualStudio.Toolkit;
    using Community.VisualStudio.Toolkit.DependencyInjection.Microsoft;
    using EnvDTE;
    using EnvDTE80;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell;
    using PackageReferenceVersionToAttribute;
    using PackageReferenceVersionToAttributeExtension.Logging;
    using PackageReferenceVersionToAttributeExtension.Services;
    using Task = System.Threading.Tasks.Task;

    /// <summary>
    /// PackageReference Version to attribute extension package.
    /// </summary>
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration(Vsix.Name, Vsix.Description, Vsix.Version)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(PackageGuids.guidPackageReferenceVersionToAttributeExtensionString)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.NoSolution_string, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionExists_string, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideUIContextRule(
        contextGuid: PackageGuids.guidPackageReferenceVersionToAttributeExtensionProjectNodeUIRuleString,
        name: "Csproj",
        expression: "Csproj",
        termNames: ["Csproj"],
        termValues: ["ActiveProjectCapability:CSharp"])]
    [ProvideUIContextRule(
        contextGuid: PackageGuids.guidPackageReferenceVersionToAttributeExtensionSolutionNodeUIRuleString,
        name: "Sln",
        expression: "(SingleProject | MultipleProjects)",
        termNames: ["SingleProject", "MultipleProjects"],
        termValues: [VSConstants.UICONTEXT.SolutionHasSingleProject_string, VSConstants.UICONTEXT.SolutionHasMultipleProjects_string])]
    public sealed class Package : MicrosoftDIToolkitPackage<Package>
    {
        /// <inheritdoc/>
        protected override void InitializeServices(IServiceCollection services)
        {
            base.InitializeServices(services);

            // register services
            services.AddSingleton<IFileService, FileSystemService>();
            services.AddSingleton<ISourceControlService, ProjectService>();

            services.AddSingleton((serviceProvider)
                => VS.GetRequiredService<DTE, DTE2>());

            services.AddSingleton<BaseCommand>();
            services.AddSingleton<ProjectService>();
            services.AddSingleton<ProjectConverter>();
            services.AddLogging(configure =>
            {
                configure.ClearProviders();
                configure.Services.AddSingleton<ILoggerProvider, CustomLoggerProvider>();
                configure.SetMinimumLevel(LogLevel.Trace);
            });

            // register commands
            services.RegisterCommands(ServiceLifetime.Singleton, Assembly.GetExecutingAssembly());
        }

        /// <inheritdoc/>
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await base.InitializeAsync(cancellationToken, progress);
        }
    }
}