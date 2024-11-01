// <copyright file="MockMultiItemSelect.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeExtensionTests.Mocks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell.Interop;
    using PackageReferenceVersionToAttributeExtensionTests.Logging;

    /// <summary>
    /// Mock multi-item selection.
    /// </summary>
    internal class MockMultiItemSelect : IVsMultiItemSelect
    {
        private readonly ILogger<MockMultiItemSelect> logger;
        private readonly List<MockProject> selections = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="MockMultiItemSelect"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public MockMultiItemSelect(ILogger<MockMultiItemSelect> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc/>
        public int GetSelectionInfo(out uint pcItems, out int pfSingleHierarchy)
        {
            pcItems = (uint)this.selections.Count;
            pfSingleHierarchy = this.selections.Count;
            return VSConstants.S_OK;
        }

        /// <inheritdoc/>
        public int GetSelectedItems(uint grfGSI, uint cItems, VSITEMSELECTION[] rgItemSel)
        {
            this.logger.LogDebug(
                $"""
                Entering GetSelectedItems with:
                    grfGSI: {grfGSI},
                    cItems: {cItems},
                    rgItemSel.Length: {rgItemSel?.Length}
                """);

            if (rgItemSel == null || rgItemSel.Length < this.selections.Count)
            {
                // the array is too small
                this.logger.LogDebug(
                    "Exiting GetSelectedItems with VSConstants.E_INVALIDARG");
                return VSConstants.E_INVALIDARG;
            }

            for (int i = 0; i < rgItemSel.Length; i++)
            {
                rgItemSel[i] = new VSITEMSELECTION
                {
                    pHier = this.selections[i],
                    itemid = VSConstants.VSITEMID_ROOT,
                };

                this.logger.LogDebug(
                    $"""
                    Set rgItemSel[{i}]:
                        pHier = {this.selections[i].ToString(1)},
                        itemid = {ItemIdFormatter.Format(rgItemSel[i].itemid)}
                    """);
            }

            this.logger.LogDebug(
                "Exiting GetSelectedItems with VSConstants.S_OK");

            return VSConstants.S_OK;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return this.ToString(1);
        }

        /// <summary>
        /// Adds the items to the selection.
        /// </summary>
        /// <param name="items">The items.</param>
        internal void AddSelections(params MockProject[] items)
        {
            this.selections.AddRange(items);
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
                {nameof(MockMultiItemSelect)}:
                    {indent}SelectedItemCount: {this.selections.Count}
                    {indent}SelectedItems: {string.Join(Environment.NewLine, this.selections.Select(x => x.ToString(indentLevel + 1)))}
                """;
        }
    }
}