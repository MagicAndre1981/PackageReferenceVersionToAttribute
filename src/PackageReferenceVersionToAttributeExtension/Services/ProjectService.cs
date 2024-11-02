// <copyright file="ProjectService.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeExtension.Services
{
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using EnvDTE80;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.Shell;
    using PackageReferenceVersionToAttribute;
    using Project = EnvDTE.Project;

    /// <summary>
    /// Provides support for operations on a project.
    /// </summary>
    /// <param name="dte">The Visual Studio automation object model.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="fileService">The file service.</param>
    public class ProjectService(
        DTE2 dte,
        ILogger<ProjectService> logger,
        IFileService fileService) : ISourceControlService
    {
        private readonly DTE2 dte = dte;
        private readonly ILogger<ProjectService> logger = logger;
        private readonly IFileService fileService = fileService;

        /// <summary>
        /// Checks out the specified file from source control.
        /// </summary>
        /// <param name="filePath">The path of the file.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task CheckOutFileAsync(string filePath)
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
                this.logger.LogDebug($"Checking out file \"{filePath}\"...");

                this.dte.SourceControl.CheckOutItem(filePath);
            }

            this.fileService.RemoveReadOnlyAttribute(filePath);
        }
    }
}
