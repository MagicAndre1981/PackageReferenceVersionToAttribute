// <copyright file="ISourceControlService.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttribute
{
    using System.Threading.Tasks;

    /// <summary>
    /// Provides source control operations, such as checking out files for modification.
    /// </summary>
    public interface ISourceControlService
    {
        /// <summary>
        /// Checks out the specified file from source control, allowing modifications.
        /// </summary>
        /// <param name="path">The path of the file to check out.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CheckOutFileAsync(string path);
    }
}
