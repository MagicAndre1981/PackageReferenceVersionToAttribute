// <copyright file="MockProjects.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeExtensionTests.Mocks
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using EnvDTE;

    /// <summary>
    /// Mock projects.
    /// </summary>
    internal class MockProjects : Projects, IEnumerable<Project>
    {
        private readonly List<Project> projects = new();

        /// <inheritdoc/>
        public DTE Parent => throw new NotImplementedException();

        /// <inheritdoc/>
        public int Count => throw new NotImplementedException();

        /// <inheritdoc/>
        public DTE DTE => throw new NotImplementedException();

        /// <inheritdoc/>
        public Properties Properties => throw new NotImplementedException();

        /// <inheritdoc/>
        public string Kind => throw new NotImplementedException();

        /// <inheritdoc/>
        public Project Item(object index)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        IEnumerator<Project> IEnumerable<Project>.GetEnumerator()
        {
            return this.projects.GetEnumerator();
        }
    }
}