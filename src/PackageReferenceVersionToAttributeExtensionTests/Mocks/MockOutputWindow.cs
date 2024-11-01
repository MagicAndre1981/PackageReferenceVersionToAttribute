// <copyright file="MockOutputWindow.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeExtensionTests.Mocks
{
    using System;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell.Interop;

    /// <summary>
    /// Mock output window.
    /// </summary>
    internal class MockOutputWindow : IVsOutputWindow, SVsOutputWindow
    {
        private readonly MockOutputWindowPane outputWindowPane;

        /// <summary>
        /// Initializes a new instance of the <see cref="MockOutputWindow"/> class.
        /// </summary>
        /// <param name="outputWindowPane">The output window pane.</param>
        public MockOutputWindow(MockOutputWindowPane outputWindowPane)
        {
            this.outputWindowPane = outputWindowPane;
        }

        /// <inheritdoc/>
        public int GetPane(ref Guid rguidPane, out IVsOutputWindowPane ppPane)
        {
            ppPane = this.outputWindowPane;

            return VSConstants.S_OK;
        }

        /// <inheritdoc/>
        public int CreatePane(ref Guid rguidPane, string pszPaneName, int fInitVisible, int fClearWithSolution)
        {
            return VSConstants.S_OK;
        }

        /// <inheritdoc/>
        public int DeletePane(ref Guid rguidPane)
        {
            throw new NotImplementedException();
        }
    }
}