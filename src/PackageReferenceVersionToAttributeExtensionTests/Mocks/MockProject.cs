// <copyright file="MockProject.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeExtensionTests.Mocks
{
    using System;
    using System.IO;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell.Interop;

    /// <summary>
    /// Mock project.
    /// </summary>
    internal sealed class MockProject : MockHierarchy, IVsProject, IDisposable
    {
        private readonly ILogger<MockProject> logger;
        private readonly TempFile tempFile = new();
        private readonly TempFile tempFileBak;
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="MockProject"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        public MockProject(ILoggerFactory loggerFactory)
            : base(loggerFactory.CreateLogger<MockHierarchy>())
        {
            this.logger = loggerFactory.CreateLogger<MockProject>();

            this.tempFileBak = new(this.tempFile.FilePath + ".bak");

            this.RootItem = new MockHierarchyItem(
                VSConstants.VSITEMID_ROOT,
                this,
                this);
        }

        /// <summary>
        /// Gets the root item.
        /// </summary>
        public MockHierarchyItem RootItem { get; }

        /// <summary>
        /// Gets the path.
        /// </summary>
        public string Path => this.tempFile.FilePath;

        /// <summary>
        /// Gets or sets the contents.
        /// </summary>
        public string Contents
        {
            get
            {
                return File.ReadAllText(this.tempFile.FilePath);
            }

            set
            {
                File.WriteAllText(
                    this.tempFile.FilePath,
                    value);
            }
        }

        /// <inheritdoc/>
        public int IsDocumentInProject(string pszMkDocument, out int pfFound, VSDOCUMENTPRIORITY[] pdwPriority, out uint pitemid)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int GetMkDocument(uint itemid, out string pbstrMkDocument)
        {
            pbstrMkDocument = string.Empty;

            this.logger.LogDebug(
                $"GetMkDocument called with itemid: {itemid}");

            if (itemid == VSConstants.VSITEMID_ROOT)
            {
                // Return the project file path for the root item.
                pbstrMkDocument = this.tempFile.FilePath;
                this.logger.LogDebug($"Returning project file path: {pbstrMkDocument}");
                return VSConstants.S_OK;
            }

            // Return an error if the item ID is not recognized.
            this.logger.LogWarning($"Item ID {itemid} not recognized. Returning error.");
            return VSConstants.E_INVALIDARG;
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
        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.tempFile.Dispose();
            this.tempFileBak.Dispose();

            this.disposed = true;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return this.ToString(1);
        }
    }
}