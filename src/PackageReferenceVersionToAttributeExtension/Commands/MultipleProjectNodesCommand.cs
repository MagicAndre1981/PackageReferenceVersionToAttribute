// <copyright file="MultipleProjectNodesCommand.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeExtension
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Community.VisualStudio.Toolkit;
    using Community.VisualStudio.Toolkit.DependencyInjection;
    using Community.VisualStudio.Toolkit.DependencyInjection.Core;
    using EnvDTE;
    using Microsoft.VisualStudio.Shell;

    /// <summary>
    /// Project node command.
    /// </summary>
    [Command(
        PackageGuids.guidPackageReferenceVersionToAttributeExtensionCmdSetString,
        PackageIds.PackageReferenceVersionToAttributeMultipleProjectNodesCommand)]
    internal sealed class MultipleProjectNodesCommand(
        DIToolkitPackage package,
        BaseCommand baseCommand)
        : BaseDICommand(package)
    {
        private readonly BaseCommand baseCommand = baseCommand;

        /// <inheritdoc/>
        protected override void BeforeQueryStatus(EventArgs e)
        {
            this.Package.JoinableTaskFactory.Run(async () =>
            {
                // Check the current selection in Solution Explorer
                var selectedProjects = await this.baseCommand.GetSelectedProjectsAsync();

                // Enable command if there are any C# projects selected
                this.Command.Enabled = selectedProjects.Any(p => p.IsCapabilityMatch("CSharp"));
            });
        }

        /// <inheritdoc/>
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await this.baseCommand.ConvertPackageReferenceVersionElementsToAttributesAsync();
        }
    }
}
