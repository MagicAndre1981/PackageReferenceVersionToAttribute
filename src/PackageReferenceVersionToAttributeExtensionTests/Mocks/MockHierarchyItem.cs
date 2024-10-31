// <copyright file="MockHierarchyItem.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeExtensionTests.Mocks
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Microsoft.VisualStudio.Shell;

    /// <summary>
    /// Mock hierarchy item.
    /// </summary>
    internal class MockHierarchyItem : IVsHierarchyItem
    {
        private readonly MockHierarchyItemIdentity hierarchyIdentity;

        /// <summary>
        /// Initializes a new instance of the <see cref="MockHierarchyItem"/> class.
        /// </summary>
        /// <param name="hierarchyIdentity">The hierarchy identity.</param>
        public MockHierarchyItem(MockHierarchyItemIdentity hierarchyIdentity)
        {
            this.hierarchyIdentity = hierarchyIdentity;
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <inheritdoc/>
        public event PropertyChangingEventHandler PropertyChanging;

        /// <inheritdoc/>
        public IVsHierarchyItemIdentity HierarchyIdentity => this.hierarchyIdentity;

        /// <inheritdoc/>
        public IVsHierarchyItem Parent => throw new NotImplementedException();

        /// <inheritdoc/>
        public IEnumerable<IVsHierarchyItem> Children => throw new NotImplementedException();

        /// <inheritdoc/>
        public bool AreChildrenRealized => throw new NotImplementedException();

        /// <inheritdoc/>
        public string Text => throw new NotImplementedException();

        /// <inheritdoc/>
        public string CanonicalName => throw new NotImplementedException();

        /// <inheritdoc/>
        public bool IsBold { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <inheritdoc/>
        public bool IsCut { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <inheritdoc/>
        public bool IsDisposed => throw new NotImplementedException();

        /// <summary>
        /// Adds the item to the hierarchy.
        /// </summary>
        /// <param name="item">The item.</param>
        internal void AddItem(MockProject item)
        {
            this.hierarchyIdentity.AddItem(item);
        }
    }
}