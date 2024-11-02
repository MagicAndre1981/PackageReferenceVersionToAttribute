// <copyright file="SolutionNodeCommand.cs" company="Rami Abughazaleh">
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

    /// <summary>
    /// Solution node command.
    /// </summary>
    [Command(
        PackageGuids.guidPackageReferenceVersionToAttributeExtensionCmdSetString,
        PackageIds.PackageReferenceVersionToAttributeSolutionNodeCommand)]
    internal sealed class SolutionNodeCommand(
        DIToolkitPackage package,
        BaseCommand baseCommand)
        : BaseDICommand(package)
    {
        private readonly BaseCommand baseCommand = baseCommand;

        /// <inheritdoc/>
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await this.baseCommand.ConvertPackageReferenceVersionElementsToAttributesAsync();
        }
    }
}
