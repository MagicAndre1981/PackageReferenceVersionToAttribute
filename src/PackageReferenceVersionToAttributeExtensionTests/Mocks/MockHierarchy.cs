// <copyright file="MockHierarchy.cs" company="Rami Abughazaleh">
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
    /// Mock hierarchy.
    /// </summary>
    internal class MockHierarchy : IVsHierarchy
    {
        private readonly ILogger<MockHierarchy> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MockHierarchy"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public MockHierarchy(ILogger<MockHierarchy> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public uint Id { get; internal set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; internal set; }

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
            pbstrName = this.Name;

            this.logger.LogDebug(
                $"""
                GetCanonicalName called with:
                    itemid: {itemid}.
                  Resulting name: {Path.GetFileNameWithoutExtension(pbstrName)}
                """);

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

        /// <inheritdoc/>
        public override string ToString()
        {
            return this.ToString(1);
        }

        /// <summary>
        /// Returns a string representation of this object, formatted with indentation based on the specified level.
        /// </summary>
        /// <param name="indentLevel">The level of indentation to apply to the output string. Defaults to 1.</param>
        /// <returns>A formatted string representing this object.</returns>
        internal string ToString(int indentLevel = 1)
        {
            string indent = new(' ', indentLevel * 4);

            return $"""
                {nameof(MockHierarchy)}:
                    {indent}Id: {this.Id}
                    {indent}Name: {this.Name}
                """;
        }
    }
}