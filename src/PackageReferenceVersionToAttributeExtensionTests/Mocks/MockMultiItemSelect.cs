// <copyright file="MockMultiItemSelect.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeExtensionTests.Mocks
{
    using System.Collections.Generic;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell.Interop;

    /// <summary>
    /// Mock multi-item selection.
    /// </summary>
    internal class MockMultiItemSelect : IVsMultiItemSelect
    {
        private List<MockProject> selections = new List<MockProject>();
        private MockHierarchy hierarchy;

        /// <summary>
        /// Initializes a new instance of the <see cref="MockMultiItemSelect"/> class.
        /// </summary>
        /// <param name="hierarchy">The hierarchy.</param>
        public MockMultiItemSelect(MockHierarchy hierarchy)
        {
            this.hierarchy = hierarchy;
        }

        /// <inheritdoc/>
        public int GetSelectionInfo(out uint pcItems, out int pfSingleHierarchy)
        {
            // Simulate a single selected item
            pcItems = 1;

            // 1 indicates a single hierarchy selection
            pfSingleHierarchy = 1;
            return VSConstants.S_OK;
        }

        /// <inheritdoc/>
        public int GetSelectedItems(uint grfGSI, uint cItems, VSITEMSELECTION[] rgItemSel)
        {
            // Ensure the array has at least one item
            if (rgItemSel != null && rgItemSel.Length > 0)
            {
                rgItemSel[0] = new VSITEMSELECTION
                {
                    // Mock hierarchy representing the selected project
                    pHier = this.hierarchy,

                    // Use VSITEMID_ROOT to represent the root node of a project
                    itemid = VSConstants.VSITEMID_ROOT,
                };
            }

            return VSConstants.S_OK;
        }

        /// <summary>
        /// Adds the item to the selection.
        /// </summary>
        /// <param name="item">The item.</param>
        internal void AddSelection(MockProject item)
        {
            this.selections.Add(item);
        }
    }
}