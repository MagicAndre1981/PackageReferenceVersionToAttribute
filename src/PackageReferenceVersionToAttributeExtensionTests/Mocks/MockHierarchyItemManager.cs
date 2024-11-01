// <copyright file="MockHierarchyItemManager.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeExtensionTests.Mocks
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;

    /// <summary>
    /// Mock hierarchy item manager.
    /// </summary>
    internal class MockHierarchyItemManager : IVsHierarchyItemManager
    {
        private readonly Dictionary<MockProject, Dictionary<uint, MockHierarchyItem>> hierarchyItems;

        private readonly ILogger<MockHierarchyItemManager> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MockHierarchyItemManager"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public MockHierarchyItemManager(
            ILogger<MockHierarchyItemManager> logger)
        {
            this.logger = logger;
            this.hierarchyItems = new Dictionary<MockProject, Dictionary<uint, MockHierarchyItem>>();
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
            if (this.hierarchyItems.TryGetValue(hierarchy as MockProject, out var items)
                && items.TryGetValue(itemid, out var item))
            {
                string hierarchyString;

                if (hierarchy is MockHierarchy mockHierarchy)
                {
                    hierarchyString = mockHierarchy.ToString(1);
                }
                else
                {
                    hierarchyString = hierarchy.ToString();
                }

                this.logger.LogDebug(
                    $"""
                    GetHierarchyItem called with:
                        hierarchy: {hierarchyString},
                        itemid: {itemid}.
                      Returning hierarchy item: {item.ToString(1)}
                    """);

                return item;
            }
            else
            {
                throw new KeyNotFoundException($"Item with ID {itemid} not found in the specified hierarchy.");
            }
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
        /// Adds the items to the hierarchy.
        /// </summary>
        /// <param name="items">The items.</param>
        internal void AddItems(params MockProject[] items)
        {
            foreach (var item in items)
            {
                if (!this.hierarchyItems.ContainsKey(item))
                {
                    this.hierarchyItems[item] = new Dictionary<uint, MockHierarchyItem>();
                }

                this.hierarchyItems[item][VSConstants.VSITEMID_ROOT] = item.RootItem;
            }
        }
    }
}