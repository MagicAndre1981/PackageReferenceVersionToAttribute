// <copyright file="MockDevelopmentToolsEnvironment.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeExtensionTests.Mocks
{
    using System;
    using EnvDTE;
    using EnvDTE80;

    /// <summary>
    /// Mock development tools environment.
    /// </summary>
    internal class MockDevelopmentToolsEnvironment : DTE, DTE2
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MockDevelopmentToolsEnvironment"/> class.
        /// </summary>
        /// <param name="solution">The solution.</param>
        public MockDevelopmentToolsEnvironment(MockDteSolution solution)
        {
            this.Solution = solution;
        }

        /// <inheritdoc/>
        public string Name => throw new NotImplementedException();

        /// <inheritdoc/>
        public string FileName => throw new NotImplementedException();

        /// <inheritdoc/>
        public string Version => throw new NotImplementedException();

        /// <inheritdoc/>
        public object CommandBars => throw new NotImplementedException();

        /// <inheritdoc/>
        public Windows Windows => throw new NotImplementedException();

        /// <inheritdoc/>
        public Events Events => throw new NotImplementedException();

        /// <inheritdoc/>
        public AddIns AddIns => throw new NotImplementedException();

        /// <inheritdoc/>
        public Window MainWindow => throw new NotImplementedException();

        /// <inheritdoc/>
        public Window ActiveWindow => throw new NotImplementedException();

        /// <inheritdoc/>
        public vsDisplay DisplayMode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <inheritdoc/>
        public Solution Solution { get; }

        /// <inheritdoc/>
        public Commands Commands => throw new NotImplementedException();

        /// <inheritdoc/>
        public SelectedItems SelectedItems => throw new NotImplementedException();

        /// <inheritdoc/>
        public string CommandLineArguments => throw new NotImplementedException();

        /// <inheritdoc/>
        public DTE DTE => throw new NotImplementedException();

        /// <inheritdoc/>
        public int LocaleID => throw new NotImplementedException();

        /// <inheritdoc/>
        public WindowConfigurations WindowConfigurations => throw new NotImplementedException();

        /// <inheritdoc/>
        public Documents Documents => throw new NotImplementedException();

        /// <inheritdoc/>
        public Document ActiveDocument => throw new NotImplementedException();

        /// <inheritdoc/>
        public Globals Globals => throw new NotImplementedException();

        /// <inheritdoc/>
        public StatusBar StatusBar => throw new NotImplementedException();

        /// <inheritdoc/>
        public string FullName => throw new NotImplementedException();

        /// <inheritdoc/>
        public bool UserControl { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <inheritdoc/>
        public ObjectExtenders ObjectExtenders => throw new NotImplementedException();

        /// <inheritdoc/>
        public Find Find => throw new NotImplementedException();

        /// <inheritdoc/>
        public vsIDEMode Mode => throw new NotImplementedException();

        /// <inheritdoc/>
        public ItemOperations ItemOperations => throw new NotImplementedException();

        /// <inheritdoc/>
        public UndoContext UndoContext => throw new NotImplementedException();

        /// <inheritdoc/>
        public Macros Macros => throw new NotImplementedException();

        /// <inheritdoc/>
        public object ActiveSolutionProjects => throw new NotImplementedException();

        /// <inheritdoc/>
        public DTE MacrosIDE => throw new NotImplementedException();

        /// <inheritdoc/>
        public string RegistryRoot => throw new NotImplementedException();

        /// <inheritdoc/>
        public DTE Application => throw new NotImplementedException();

        /// <inheritdoc/>
        public ContextAttributes ContextAttributes => throw new NotImplementedException();

        /// <inheritdoc/>
        public SourceControl SourceControl => throw new NotImplementedException();

        /// <inheritdoc/>
        public bool SuppressUI { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <inheritdoc/>
        public Debugger Debugger => throw new NotImplementedException();

        /// <inheritdoc/>
        public string Edition => throw new NotImplementedException();

        /// <inheritdoc/>
        public ToolWindows ToolWindows => throw new NotImplementedException();

        /// <inheritdoc/>
        public void Quit()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public object GetObject(string Name)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Window OpenFile(string ViewKind, string FileName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void ExecuteCommand(string CommandName, string CommandArgs = "")
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public wizardResult LaunchWizard(string VSZFile, ref object[] ContextParams)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public string SatelliteDllPath(string Path, string Name)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Properties get_Properties(string Category, string Page)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool get_IsOpenFile(string ViewKind, string FileName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public uint GetThemeColor(vsThemeColors Element)
        {
            throw new NotImplementedException();
        }
    }
}