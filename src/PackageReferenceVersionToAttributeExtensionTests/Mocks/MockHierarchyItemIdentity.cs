// <copyright file="MockHierarchyItemIdentity.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeExtensionTests.Mocks
{
    using System;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;
    using PackageReferenceVersionToAttributeExtensionTests.Logging;

    /// <summary>
    /// Mock hierarchy item identity.
    /// </summary>
    internal class MockHierarchyItemIdentity : IVsHierarchyItemIdentity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MockHierarchyItemIdentity"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="hierarchy">The hierarchy.</param>
        /// <param name="nestedHierarchy">The nested hierarchy.</param>
        public MockHierarchyItemIdentity(uint id, IVsHierarchy hierarchy, IVsHierarchy nestedHierarchy = null)
        {
            this.ItemID = id;
            this.Hierarchy = hierarchy;
            this.NestedHierarchy = nestedHierarchy;
        }

        /// <inheritdoc/>
        public bool IsNestedItem => false;

        /// <inheritdoc/>
        public IVsHierarchy Hierarchy { get; set; }

        /// <inheritdoc/>
        public uint ItemID { get; set; }

        /// <inheritdoc/>
        public IVsHierarchy NestedHierarchy { get; set; }

        /// <inheritdoc/>
        public uint NestedItemID => (uint)VSConstants.VSITEMID.Root;

        /// <inheritdoc/>
        public bool IsRoot => throw new NotImplementedException();

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
                {nameof(MockHierarchyItemIdentity)}:
                    {indent}ItemId: {ItemIdFormatter.Format(this.ItemID)},
                    {indent}Hierarchy: {(this.Hierarchy as MockHierarchy).ToString(indentLevel + 1)},
                    {indent}NestedItemID: {ItemIdFormatter.Format(this.NestedItemID)},
                    {indent}NestedHierarchy: {(this.NestedHierarchy as MockHierarchy).ToString(indentLevel + 1)}
                """;
        }
    }
}