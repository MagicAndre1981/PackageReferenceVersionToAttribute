// <copyright file="MockHierarchyItem.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeExtensionTests.Mocks
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;

    /// <summary>
    /// Mock hierarchy item.
    /// </summary>
    internal class MockHierarchyItem : IVsHierarchyItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MockHierarchyItem"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="hierarchy">The hierarchy.</param>
        /// <param name="nestedHierarchy">The nested hierarchy.</param>
        public MockHierarchyItem(
            uint id,
            IVsHierarchy hierarchy,
            IVsHierarchy nestedHierarchy)
        {
            this.HierarchyIdentity = new MockHierarchyItemIdentity(
                id,
                hierarchy,
                nestedHierarchy);
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <inheritdoc/>
        public event PropertyChangingEventHandler PropertyChanging;

        /// <inheritdoc/>
        public IVsHierarchyItemIdentity HierarchyIdentity { get; }

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
                {nameof(MockHierarchyItem)}:
                    {indent}HierarchyIdentity: {(this.HierarchyIdentity as MockHierarchyItemIdentity).ToString(indentLevel + 1)}
                """;
        }
    }
}