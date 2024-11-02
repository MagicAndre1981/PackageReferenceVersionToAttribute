// <copyright file="ITempItem.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeToolTests.FileSystem
{
    /// <summary>
    /// Represents a temporary item that requires disposal after use.
    /// </summary>
    internal interface ITempItem : IDisposable
    {
        /// <summary>
        /// Gets the path.
        /// </summary>
        string Path { get; }
    }
}
