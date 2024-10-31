// <copyright file="MockHierarchyItemManager.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeExtensionTests.Mocks
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;

    /// <summary>
    /// Mock hierarchy item manager.
    /// </summary>
    internal class MockHierarchyItemManager : IVsHierarchyItemManager
    {
        private readonly MockHierarchyItem hierarchyItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="MockHierarchyItemManager"/> class.
        /// </summary>
        /// <param name="hierarchyItem">The hierarchy item.</param>
        public MockHierarchyItemManager(MockHierarchyItem hierarchyItem)
        {
            this.hierarchyItem = hierarchyItem;
        }

        /// <inheritdoc/>
        public event EventHandler<HierarchyItemEventArgs> AfterInvalidateItems;

        /// <inheritdoc/>
        public event EventHandler<HierarchyItemEventArgs> OnItemAdded;

        /// <inheritdoc/>
        public bool IsChangingItems => throw new NotImplementedException();

        /// <inheritdoc/>
        public IVsHierarchyItem GetHierarchyItem(IVsHierarchy hierarchy, uint itemid)
        {
            return this.hierarchyItem;
        }

        /// <inheritdoc/>
        public bool TryGetHierarchyItem(IVsHierarchy hierarchy, uint itemid, out IVsHierarchyItem item)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool TryGetHierarchyItemIdentity(IVsHierarchy hierarchy, uint itemid, out IVsHierarchyItemIdentity identity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds the item to the hierarchy.
        /// </summary>
        /// <param name="item">The item.</param>
        internal void AddItem(MockProject item)
        {
            this.hierarchyItem.AddItem(item);
        }
    }
}