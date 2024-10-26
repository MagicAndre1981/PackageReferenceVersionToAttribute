// <copyright file="ConvertPackageReferenceVersionElementsToAttributesCommand.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeExtension
{
    using System.Threading.Tasks;
    using Community.VisualStudio.Toolkit;
    using EnvDTE;
    using Microsoft.VisualStudio.Shell;

    /// <summary>
    /// Convert PackageReference Version elements to attributes command.
    /// </summary>
    [Command(
        PackageGuids.guidPackageReferenceVersionToAttributeExtensionCmdSetString,
        PackageIds.PackageReferenceVersionToAttributeCommand)]
    internal sealed class ConvertPackageReferenceVersionElementsToAttributesCommand : BaseCommand<ConvertPackageReferenceVersionElementsToAttributesCommand>
    {
        /// <inheritdoc/>
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await VS.MessageBox.ShowWarningAsync("PackageReferenceVersionToAttributeExtension", "Button clicked");
        }
    }
}
