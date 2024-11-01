// <copyright file="ItemIdFormatter.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeExtensionTests.Logging
{
    using Microsoft.VisualStudio;

    /// <summary>
    /// Item id formatter.
    /// </summary>
    public static class ItemIdFormatter
    {
        /// <summary>
        /// Formats the given item ID into a human-readable string.
        /// Returns specific names for predefined item IDs and the string representation
        /// of the item ID for any other values.
        /// </summary>
        /// <param name="itemid">The item ID to format.</param>
        /// <returns>A string representation of the item ID.</returns>
        public static string Format(uint itemid)
        {
            return itemid switch
            {
                VSConstants.VSITEMID_ROOT => "VSITEMID_ROOT",
                VSConstants.VSITEMID_SELECTION => "VSITEMID_SELECTION",
                VSConstants.VSITEMID_NIL => "VSITEMID_NIL",
                _ => itemid.ToString(),
            };
        }
    }
}
