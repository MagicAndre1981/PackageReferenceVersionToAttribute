// <copyright file="MockSolution.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeExtensionTests.Mocks
{
    using System;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell.Interop;

    /// <summary>
    /// Mock solution.
    /// </summary>
    internal class MockSolution : IVsSolution, IVsHierarchy
    {
        /// <inheritdoc/>
        public int GetProjectEnum(uint grfEnumFlags, ref Guid rguidEnumOnlyThisType, out IEnumHierarchies ppenum)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int CreateProject(ref Guid rguidProjectType, string lpszMoniker, string lpszLocation, string lpszName, uint grfCreateFlags, ref Guid iidProject, out IntPtr ppProject)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int GenerateUniqueProjectName(string lpszRoot, out string pbstrProjectName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int GetProjectOfGuid(ref Guid rguidProjectID, out IVsHierarchy ppHierarchy)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int GetGuidOfProject(IVsHierarchy pHierarchy, out Guid pguidProjectID)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int GetSolutionInfo(out string pbstrSolutionDirectory, out string pbstrSolutionFile, out string pbstrUserOptsFile)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int AdviseSolutionEvents(IVsSolutionEvents pSink, out uint pdwCookie)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int UnadviseSolutionEvents(uint dwCookie)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int SaveSolutionElement(uint grfSaveOpts, IVsHierarchy pHier, uint docCookie)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int CloseSolutionElement(uint grfCloseOpts, IVsHierarchy pHier, uint docCookie)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int GetProjectOfProjref(string pszProjref, out IVsHierarchy ppHierarchy, out string pbstrUpdatedProjref, VSUPDATEPROJREFREASON[] puprUpdateReason)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int GetProjrefOfProject(IVsHierarchy pHierarchy, out string pbstrProjref)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int GetProjectInfoOfProjref(string pszProjref, int propid, out object pvar)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int AddVirtualProject(IVsHierarchy pHierarchy, uint grfAddVPFlags)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int GetItemOfProjref(string pszProjref, out IVsHierarchy ppHierarchy, out uint pitemid, out string pbstrUpdatedProjref, VSUPDATEPROJREFREASON[] puprUpdateReason)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int GetProjrefOfItem(IVsHierarchy pHierarchy, uint itemid, out string pbstrProjref)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int GetItemInfoOfProjref(string pszProjref, int propid, out object pvar)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int GetProjectOfUniqueName(string pszUniqueName, out IVsHierarchy ppHierarchy)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int GetUniqueNameOfProject(IVsHierarchy pHierarchy, out string pbstrUniqueName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int GetProperty(int propid, out object pvar)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int SetProperty(int propid, object var)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int OpenSolutionFile(uint grfOpenOpts, string pszFilename)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int QueryEditSolutionFile(out uint pdwEditResult)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int CreateSolution(string lpszLocation, string lpszName, uint grfCreateFlags)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int GetProjectFactory(uint dwReserved, Guid[] pguidProjectType, string pszMkProject, out IVsProjectFactory ppProjectFactory)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int GetProjectTypeGuid(uint dwReserved, string pszMkProject, out Guid pguidProjectType)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int OpenSolutionViaDlg(string pszStartDirectory, int fDefaultToAllProjectsFilter)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int AddVirtualProjectEx(IVsHierarchy pHierarchy, uint grfAddVPFlags, ref Guid rguidProjectID)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int QueryRenameProject(IVsProject pProject, string pszMkOldName, string pszMkNewName, uint dwReserved, out int pfRenameCanContinue)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int OnAfterRenameProject(IVsProject pProject, string pszMkOldName, string pszMkNewName, uint dwReserved)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int RemoveVirtualProject(IVsHierarchy pHierarchy, uint grfRemoveVPFlags)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int CreateNewProjectViaDlg(string pszExpand, string pszSelect, uint dwReserved)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int GetVirtualProjectFlags(IVsHierarchy pHierarchy, out uint pgrfAddVPFlags)
        {
            pgrfAddVPFlags = 0;
            return VSConstants.E_FAIL;
        }

        /// <inheritdoc/>
        public int GenerateNextDefaultProjectName(string pszBaseName, string pszLocation, out string pbstrProjectName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int GetProjectFilesInSolution(uint grfGetOpts, uint cProjects, string[] rgbstrProjectNames, out uint pcProjectsFetched)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int CanCreateNewProjectAtLocation(int fCreateNewSolution, string pszFullProjectFilePath, out int pfCanCreate)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int SetSite(Microsoft.VisualStudio.OLE.Interop.IServiceProvider psp)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int GetSite(out Microsoft.VisualStudio.OLE.Interop.IServiceProvider ppSP)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int QueryClose(out int pfCanClose)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int Close()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int GetGuidProperty(uint itemid, int propid, out Guid pguid)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int SetGuidProperty(uint itemid, int propid, ref Guid rguid)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int GetProperty(uint itemid, int propid, out object pvar)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int SetProperty(uint itemid, int propid, object var)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int GetNestedHierarchy(uint itemid, ref Guid iidHierarchyNested, out IntPtr ppHierarchyNested, out uint pitemidNested)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int GetCanonicalName(uint itemid, out string pbstrName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int ParseCanonicalName(string pszName, out uint pitemid)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int Unused0()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int AdviseHierarchyEvents(IVsHierarchyEvents pEventSink, out uint pdwCookie)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int UnadviseHierarchyEvents(uint dwCookie)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int Unused1()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int Unused2()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int Unused3()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int Unused4()
        {
            throw new NotImplementedException();
        }
    }
}