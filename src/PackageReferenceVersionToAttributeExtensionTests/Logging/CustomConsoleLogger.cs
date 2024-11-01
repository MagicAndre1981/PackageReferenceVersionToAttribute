// <copyright file="CustomConsoleLogger.cs" company="Rami Abughazaleh">
//   Copyright (c) Rami Abughazaleh. All rights reserved.
// </copyright>

namespace PackageReferenceVersionToAttributeExtensionTests.Logging
{
    using System;
    using System.Linq;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Custom console logger.
    /// </summary>
    internal class CustomConsoleLogger : ILogger
    {
        private readonly string name;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomConsoleLogger"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public CustomConsoleLogger(string name)
        {
            this.name = name.Split('.').Last();
        }

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

                const int nameWidth = 25;

                var lines = message.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

                Console.WriteLine($"{logLevel}: {this.name,-nameWidth}: {lines[0]}");

                if (lines.Length > 1)
                {
                    for (int i = 1; i < lines.Length; i++)
                    {
                        Console.WriteLine($"{new string(' ', logLevel.ToString().Length + nameWidth + 2)}{lines[i]}");
                    }
                }
            }
        }
    }
}
