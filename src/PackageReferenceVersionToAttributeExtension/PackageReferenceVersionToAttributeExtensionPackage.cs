// <copyright file="PackageReferenceVersionToAttributeExtensionPackage.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeExtension
{
    using System;
    using System.Runtime.InteropServices;
    using System.Threading;
    using Community.VisualStudio.Toolkit;
    using Microsoft.VisualStudio.Shell;
    using Task = System.Threading.Tasks.Task;

    /// <summary>
    /// PackageReference Version to attribute extension package.
    /// </summary>
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration(Vsix.Name, Vsix.Description, Vsix.Version)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(PackageGuids.guidPackageReferenceVersionToAttributeExtensionString)]
    [ProvideUIContextRule(
        contextGuid: PackageGuids.guidPackageReferenceVersionToAttributeExtensionUIRuleString,
        name: "Csproj",
        expression: "Csproj",
        termNames: ["Csproj"],
        termValues: ["ActiveProjectCapability:CSharp"])]
    public sealed class PackageReferenceVersionToAttributeExtensionPackage : ToolkitPackage
    {
        /// <inheritdoc/>
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await this.RegisterCommandsAsync();
        }
    }
}