// <copyright file="ProjectNodeCommand.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeExtension
{
    using System.Threading.Tasks;
    using Community.VisualStudio.Toolkit;
    using Community.VisualStudio.Toolkit.DependencyInjection;
    using Community.VisualStudio.Toolkit.DependencyInjection.Core;
    using EnvDTE;
    using Microsoft.VisualStudio.Shell;
    using PackageReferenceVersionToAttributeExtension.Services;

    /// <summary>
    /// Project node command.
    /// </summary>
    [Command(
        PackageGuids.guidPackageReferenceVersionToAttributeExtensionCmdSetString,
        PackageIds.PackageReferenceVersionToAttributeProjectNodeCommand)]
    internal sealed class ProjectNodeCommand(
        DIToolkitPackage package,
        LoggingService loggingService,
        ProjectService projectService,
        FileSystemService fileSystemService)
        : BaseDICommand(package)
    {
        private readonly BaseCommand baseCommand = new(loggingService, projectService, fileSystemService);

        /// <inheritdoc/>
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await this.baseCommand.ConvertPackageReferenceVersionElementsToAttributesAsync();
        }
    }
}
