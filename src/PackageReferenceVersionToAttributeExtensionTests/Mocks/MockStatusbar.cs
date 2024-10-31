// <copyright file="MockStatusbar.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeExtensionTests.Mocks
{
    using System;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell.Interop;

    /// <summary>
    /// Mock status bar.
    /// </summary>
    internal class MockStatusbar : IVsStatusbar
    {
        /// <inheritdoc/>
        public int Clear()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int SetText(string pszText)
        {
            Console.WriteLine(pszText);
            return VSConstants.S_OK;
        }

        /// <inheritdoc/>
        public int Progress(ref uint pdwCookie, int fInProgress, string pwszLabel, uint nComplete, uint nTotal)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int Animation(int fOnOff, ref object pvIcon)
        {
            return VSConstants.S_OK;
        }

        /// <inheritdoc/>
        public int SetSelMode(ref object pvSelMode)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int SetInsMode(ref object pvInsMode)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int SetLineChar(ref object pvLine, ref object pvChar)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int SetXYWH(ref object pvX, ref object pvY, ref object pvW, ref object pvH)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int SetLineColChar(ref object pvLine, ref object pvCol, ref object pvChar)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int IsCurrentUser(IVsStatusbarUser pUser, ref int pfCurrent)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int SetColorText(string pszText, uint crForeColor, uint crBackColor)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int GetText(out string pszText)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int FreezeOutput(int fFreeze)
        {
            return VSConstants.S_OK;
        }

        /// <inheritdoc/>
        public int IsFrozen(out int pfFrozen)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int GetFreezeCount(out int plCount)
        {
            throw new NotImplementedException();
        }
    }
}
