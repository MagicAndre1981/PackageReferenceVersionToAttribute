// <copyright file="TempDir.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeToolTests.FileSystem
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Represents a temporary directory that can contain temporary items.
    /// </summary>
    internal sealed class TempDir : List<ITempItem>, ITempItem
    {
        private bool pathCreated;

        /// <summary>
        /// Initializes a new instance of the <see cref="TempDir"/> class.
        /// </summary>
        /// <param name="name">The name of the directory.</param>
        public TempDir(string name = null)
        {
            this.Name = name;
            if (this.Name == null)
            {
                // this is the parent
                this.Path = System.IO.Path.Combine(
                    System.IO.Path.GetTempPath(),
                    System.IO.Path.GetRandomFileName());
            }

            this.pathCreated = false;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the path.
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// Adds a temporary item (file or directory) to the current temporary directory.
        /// If the item is added to the root directory, it creates the necessary
        /// directory structure and writes the item to disk.
        /// </summary>
        /// <param name="item">The temporary item to add, which can be a file or a subdirectory.</param>
        public new void Add(ITempItem item)
        {
            base.Add(item);

            // Only set paths and create directories/files if adding to the root TempDir
            if (this.Path != null && this.Name == null)
            {
                // Ensure the root directory is created
                this.EnsurePathCreated();

                this.ProcessItem(item);
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            // Cleanup of the temp directory and all contained files/folders
            foreach (var child in this)
            {
                child.Dispose();
            }

            if (Directory.Exists(this.Path))
            {
                Console.WriteLine($"Deleting directory: {this.Path}");
                Directory.Delete(this.Path, true);
            }
        }

        // Recursive method to process items in the TempDir
        private void ProcessItem(ITempItem item)
        {
            if (item is TempFile tempFile)
            {
                // Create the file and write its contents
                var filePath = System.IO.Path.Combine(this.Path, tempFile.Name);
                tempFile.Path = filePath; // Set the path of the TempFile
                tempFile.WriteAllText();
            }
            else if (item is TempDir tempDir)
            {
                // Create the subdirectory and set its path
                var dirPath = System.IO.Path.Combine(this.Path, tempDir.Name);
                tempDir.Path = dirPath; // Set the path of the TempDir
                tempDir.EnsurePathCreated();

                // Recursively process each child item in the TempDir
                foreach (var childItem in tempDir)
                {
                    // Call the recursive method for the child item
                    tempDir.ProcessItem(childItem);
                }
            }
        }

        // Ensure that the directory path exists (lazy creation)
        private void EnsurePathCreated()
        {
            if (!this.pathCreated)
            {
                Console.WriteLine($"Creating directory: {this.Path}");
                Directory.CreateDirectory(this.Path);
                this.pathCreated = true;
            }
        }
    }
}