// <copyright file="TempFile.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeToolTests.FileSystem
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TempFile"/> class.
    /// </summary>
    /// <param name="name">The name of the file.</param>
    /// <param name="contents">The contents of the file.</param>
    internal sealed class TempFile(string name, string contents) : ITempItem
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; } = name;

        /// <summary>
        /// Gets the contents.
        /// </summary>
        public string Contents { get; } = contents;

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        public string Path { get; set; }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (File.Exists(this.Path))
            {
                Console.WriteLine($"Deleting file: {this.Path}");
                File.Delete(this.Path);
            }
        }

        /// <summary>
        /// Writes the contents to the file specified by the <see cref="Path"/> property.
        /// This method will create the file if it does not exist or overwrite it if it does.
        /// </summary>
        internal void WriteAllText()
        {
            Console.WriteLine($"Writing file: {this.Path}");
            File.WriteAllText(this.Path, this.Contents);
        }

        /// <summary>
        /// Reads the contents of the file specified by the <see cref="Path"/> property.
        /// This method returns the contents of the file as a string.
        /// </summary>
        /// <returns>The contents of the file.</returns>
        internal string ReadAllText()
        {
            Console.WriteLine($"Reading file: {this.Path}");
            return File.ReadAllText(this.Path);
        }
    }
}
