// <copyright file="MockOutputWindowPane.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeExtensionTests.Mocks
{
    using System;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell.Interop;

    /// <summary>
    /// Mock output window pane.
    /// </summary>
    internal class MockOutputWindowPane : IVsOutputWindowPane, SVsGeneralOutputWindowPane
    {
        /// <inheritdoc/>
        public int OutputString(string pszOutputString)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int Activate()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int Hide()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int Clear()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int FlushToTaskList()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int OutputTaskItemString(string pszOutputString, VSTASKPRIORITY nPriority, VSTASKCATEGORY nCategory, string pszSubcategory, int nBitmap, string pszFilename, uint nLineNum, string pszTaskItemText)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int OutputTaskItemStringEx(string pszOutputString, VSTASKPRIORITY nPriority, VSTASKCATEGORY nCategory, string pszSubcategory, int nBitmap, string pszFilename, uint nLineNum, string pszTaskItemText, string pszLookupKwd)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int GetName(ref string pbstrPaneName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int SetName(string pszPaneName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int OutputStringThreadSafe(string pszOutputString)
        {
            Console.WriteLine(pszOutputString);

            return VSConstants.S_OK;
        }
    }
}