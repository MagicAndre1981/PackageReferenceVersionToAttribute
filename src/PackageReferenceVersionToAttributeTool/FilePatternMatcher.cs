// <copyright file="FilePatternMatcher.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeTool
{
    using System.Collections.Generic;
    using System.IO;
    using Microsoft.Extensions.FileSystemGlobbing;
    using Microsoft.Extensions.FileSystemGlobbing.Abstractions;

    /// <summary>
    ///  File pattern matcher.
    /// </summary>
    internal class FilePatternMatcher
    {
        private readonly Matcher matcher = new();

        /// <summary>
        /// Retrieves a list of files that match the specified file pattern.
        /// The pattern can include wildcard characters. If the pattern is a full path
        /// to a file, it adds it to the list of matching files.
        /// </summary>
        /// <param name="filePattern">The file pattern to match against, which can include wildcards.</param>
        /// <returns>A list of matching file paths.</returns>
        public List<string> GetMatchingFiles(string filePattern)
        {
            // Convert to absolute path if relative
            if (!Path.IsPathRooted(filePattern))
            {
                filePattern = Path.Combine(Directory.GetCurrentDirectory(), filePattern);
            }

            // If there's no wildcard, return
            if (!filePattern.Contains('*'))
            {
                return [filePattern];
            }

            // Split the pattern into a directory and search pattern
            var (searchDir, searchPattern) = SplitFilePattern(filePattern);

            this.matcher.AddInclude(searchPattern);

            // Execute the pattern matching and return results
            return this.GetMatchedFiles(searchDir);
        }

        private static (string SearchDir, string Pattern) SplitFilePattern(string filePattern)
        {
            var wildCardIndex = filePattern.IndexOfAny(['*']);
            var lastSeparatorIndex = filePattern.LastIndexOf(Path.DirectorySeparatorChar, wildCardIndex);

            var searchDir = filePattern[..lastSeparatorIndex];
            var pattern = filePattern[(lastSeparatorIndex + 1)..];
            return (searchDir, pattern);
        }

        private List<string> GetMatchedFiles(string searchDir)
        {
            var directoryInfo = new DirectoryInfoWrapper(new DirectoryInfo(searchDir));
            var matches = this.matcher.Execute(directoryInfo);
            return matches.Files
                .Select(x => Path.Combine(searchDir, x.Path)
                .Replace('/', '\\'))
                .ToList();
        }
    }
}