// <copyright file="MockHierarchyItemIdentity.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeExtensionTests.Mocks
{
    using System;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;

    /// <summary>
    /// Mock hierarchy item identity.
    /// </summary>
    internal class MockHierarchyItemIdentity : IVsHierarchyItemIdentity
    {
        private readonly MockHierarchy hierarchy;
        private MockProject nestedHierarchy;

        /// <summary>
        /// Initializes a new instance of the <see cref="MockHierarchyItemIdentity"/> class.
        /// </summary>
        /// <param name="hierarchy">The hierarchy.</param>
        public MockHierarchyItemIdentity(MockHierarchy hierarchy)
        {
            this.hierarchy = hierarchy;
        }

        /// <inheritdoc/>
        public bool IsNestedItem => false;

        /// <inheritdoc/>
        public IVsHierarchy Hierarchy => this.hierarchy;

        /// <inheritdoc/>
        public uint ItemID => 0;

        /// <inheritdoc/>
        public IVsHierarchy NestedHierarchy => this.nestedHierarchy;

        /// <inheritdoc/>
        public uint NestedItemID => 4294967294u;

        /// <inheritdoc/>
        public bool IsRoot => throw new NotImplementedException();

        /// <summary>
        /// Adds the item to the hierarchy.
        /// </summary>
        /// <param name="item">The item.</param>
        internal void AddItem(MockProject item)
        {
            this.hierarchy.AddItem(item);
            this.nestedHierarchy = item;
        }
    }
}