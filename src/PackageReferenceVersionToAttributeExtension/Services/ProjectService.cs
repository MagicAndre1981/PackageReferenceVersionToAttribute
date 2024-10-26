// <copyright file="ProjectService.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeExtension.Services
{
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using EnvDTE80;
    using Microsoft.VisualStudio.Shell;
    using Project = EnvDTE.Project;

    /// <summary>
    /// Provides support for operations on a project.
    /// </summary>
    /// <param name="dte">The Visual Studio automation object model.</param>
    /// <param name="loggingService">The logging service.</param>
    /// <param name="fileSystemService">The file service.</param>
    public class ProjectService(
        DTE2 dte,
        LoggingService loggingService,
        FileSystemService fileSystemService)
    {
        private readonly DTE2 dte = dte;
        private readonly LoggingService loggingService = loggingService;
        private readonly FileSystemService fileSystemService = fileSystemService;

        /// <summary>
        /// Checks out the specified file from source control.
        /// </summary>
        /// <param name="filePath">The path of the file.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        internal async Task CheckOutFileFromSourceControlAsync(string filePath)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            if (!File.Exists(filePath)
                || ((this.dte.Solution.FindProjectItem(filePath) == null)
                    && (!this.dte.Solution.Projects.Cast<Project>().Any(x =>
                    {
                        ThreadHelper.ThrowIfNotOnUIThread();
                        return x.FileName == filePath;
                    }))))
            {
                return;
            }

            if (this.dte.SourceControl.IsItemUnderSCC(filePath)
                && !this.dte.SourceControl.IsItemCheckedOut(filePath))
            {
                await this.loggingService.LogDebugAsync($"Checking out file \"{filePath}\"...");

                this.dte.SourceControl.CheckOutItem(filePath);
            }

            await this.fileSystemService.RemoveReadOnlyAttributeAsync(filePath);
        }
    }
}
