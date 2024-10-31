// <copyright file="MockDteSolution.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeExtensionTests.Mocks
{
    using System;
    using System.Collections;
    using EnvDTE;

    /// <summary>
    /// Mock DTE solution.
    /// </summary>
    internal class MockDteSolution : Solution
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MockDteSolution"/> class.
        /// </summary>
        /// <param name="projects">The projects.</param>
        public MockDteSolution(MockProjects projects)
        {
            this.Projects = projects;
        }

        /// <inheritdoc/>
        public DTE DTE => throw new NotImplementedException();

        /// <inheritdoc/>
        public DTE Parent => throw new NotImplementedException();

        /// <inheritdoc/>
        public int Count => throw new NotImplementedException();

        /// <inheritdoc/>
        public string FileName => throw new NotImplementedException();

        /// <inheritdoc/>
        public Properties Properties => throw new NotImplementedException();

        /// <inheritdoc/>
        public bool IsDirty { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <inheritdoc/>
        public string FullName => throw new NotImplementedException();

        /// <inheritdoc/>
        public bool Saved { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <inheritdoc/>
        public Globals Globals => throw new NotImplementedException();

        /// <inheritdoc/>
        public AddIns AddIns => throw new NotImplementedException();

        /// <inheritdoc/>
        public object ExtenderNames => throw new NotImplementedException();

        /// <inheritdoc/>
        public string ExtenderCATID => throw new NotImplementedException();

        /// <inheritdoc/>
        public bool IsOpen => throw new NotImplementedException();

        /// <inheritdoc/>
        public SolutionBuild SolutionBuild => throw new NotImplementedException();

        /// <inheritdoc/>
        public Projects Projects { get; }

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
        public void SaveAs(string fileName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Project AddFromTemplate(string fileName, string destination, string projectName, bool exclusive = false)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Project AddFromFile(string fileName, bool exclusive = false)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Open(string fileName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Close(bool saveFirst = false)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Remove(Project proj)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Create(string destination, string name)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ProjectItem FindProjectItem(string fileName)
        {
            return null;
        }

        /// <inheritdoc/>
        public string ProjectItemsTemplatePath(string projectKind)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public string get_TemplatePath(string projectType)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public object get_Extender(string extenderName)
        {
            throw new NotImplementedException();
        }
    }
}