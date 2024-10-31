// <copyright file="MockHierarchy.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeExtensionTests.Mocks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell.Interop;

    /// <summary>
    /// Mock hierarchy.
    /// </summary>
    internal class MockHierarchy : IVsHierarchy
    {
        private List<MockProject> items = new List<MockProject>();

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
            pbstrName = this.items.Single().Name;

            return VSConstants.S_OK;
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
        /// Adds the item to the hierarchy.
        /// </summary>
        /// <param name="item">The item.</param>
        internal void AddItem(MockProject item)
        {
            this.items.Add(item);
        }
    }
}