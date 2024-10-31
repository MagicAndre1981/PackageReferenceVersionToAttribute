// <copyright file="MockProject.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeExtensionTests.Mocks
{
    using System;
    using System.IO;
    using Microsoft.VisualStudio.Shell.Interop;

    /// <summary>
    /// Mock project.
    /// </summary>
    internal class MockProject : IVsProject, IVsHierarchy
    {
        private readonly TempFile tempFile = new();
        private readonly TempFile tempFileBak;

        /// <summary>
        /// Initializes a new instance of the <see cref="MockProject"/> class.
        /// </summary>
        /// <param name="contents">The contents.</param>
        public MockProject(string contents)
        {
            this.tempFileBak = new(this.tempFile.FilePath + ".bak");

            File.WriteAllText(
                this.tempFile.FilePath,
                contents);
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name => this.tempFile.FilePath;

        /// <inheritdoc/>
        public int IsDocumentInProject(string pszMkDocument, out int pfFound, VSDOCUMENTPRIORITY[] pdwPriority, out uint pitemid)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int GetMkDocument(uint itemid, out string pbstrMkDocument)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int OpenItem(uint itemid, ref Guid rguidLogicalView, IntPtr punkDocDataExisting, out IVsWindowFrame ppWindowFrame)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int GetItemContext(uint itemid, out Microsoft.VisualStudio.OLE.Interop.IServiceProvider ppSP)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int GenerateUniqueItemName(uint itemidLoc, string pszExt, string pszSuggestedRoot, out string pbstrItemName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int AddItem(uint itemidLoc, VSADDITEMOPERATION dwAddItemOperation, string pszItemName, uint cFilesToOpen, string[] rgpszFilesToOpen, IntPtr hwndDlgOwner, VSADDRESULT[] pResult)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int SetSite(Microsoft.VisualStudio.OLE.Interop.IServiceProvider psp)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int GetSite(out Microsoft.VisualStudio.OLE.Interop.IServiceProvider ppSP)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int QueryClose(out int pfCanClose)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int Close()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int GetGuidProperty(uint itemid, int propid, out Guid pguid)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int SetGuidProperty(uint itemid, int propid, ref Guid rguid)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int GetProperty(uint itemid, int propid, out object pvar)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int SetProperty(uint itemid, int propid, object var)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int GetNestedHierarchy(uint itemid, ref Guid iidHierarchyNested, out IntPtr ppHierarchyNested, out uint pitemidNested)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int GetCanonicalName(uint itemid, out string pbstrName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int ParseCanonicalName(string pszName, out uint pitemid)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int Unused0()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int AdviseHierarchyEvents(IVsHierarchyEvents pEventSink, out uint pdwCookie)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int UnadviseHierarchyEvents(uint dwCookie)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int Unused1()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int Unused2()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int Unused3()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int Unused4()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads the contents of the file.
        /// </summary>
        /// <returns>The contents of the file.</returns>
        internal string ReadFile()
        {
            return File.ReadAllText(this.tempFile.FilePath);
        }
    }
}