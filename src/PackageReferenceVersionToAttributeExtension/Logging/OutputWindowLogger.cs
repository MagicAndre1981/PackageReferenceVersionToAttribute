// <copyright file="OutputWindowLogger.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeExtension.Services
{
    using System;
    using System.Diagnostics;
    using Community.VisualStudio.Toolkit;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.Shell;

    /// <summary>
    /// Provides support for logging messages to the Visual Studio output window.
    /// </summary>
    public class OutputWindowLogger() : ILogger
    {
        private const string OutputWindowPaneName = "PackageReferences Version to Attribute Extension";
        private OutputWindowPane pane;

        /// <inheritdoc/>
        public IDisposable BeginScope<TState>(TState state) => null;

        /// <inheritdoc/>
        public bool IsEnabled(LogLevel logLevel) => true;

        /// <inheritdoc/>
        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (formatter != null)
            {
                var message = formatter(state, exception);
                if (exception != null)
                {
                    message += " " + exception.ToString();
                }

                try
                {
                    ThreadHelper.JoinableTaskFactory.Run(async () =>
                    {
                        this.pane ??= await VS.Windows.CreateOutputWindowPaneAsync(OutputWindowPaneName);
                        await this.pane?.WriteLineAsync($"{DateTime.Now}: {logLevel}: {message}");

                        await VS.StatusBar.ShowMessageAsync(message);
                    });
                }
                catch (Exception ex)
                {
                    Debug.Write(ex);
                }
            }
        }
    }
}
