// <copyright file="FileLogger.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ModelViewController.Services
{
    using System;
    using System.IO;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// FileLogger class.
    /// </summary>
    public class FileLogger : ILogger
    {
        private readonly string filePath;
        private static object _lock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="FileLogger"/> class.
        /// </summary>
        /// <param name="path">path/</param>
        public FileLogger(string path)
        {
            this.filePath = path;
        }

        /// <summary>
        /// BeginScope method.
        /// </summary>
        /// <typeparam name="TState">TState.</typeparam>
        /// <param name="state">state.</param>
        /// <returns>IDisposable.</returns>
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        /// <summary>
        /// IsEnabled method.
        /// </summary>
        /// <param name="logLevel">logLevel.</param>
        /// <returns>bool.</returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        /// <summary>
        /// Log method.
        /// </summary>
        /// <typeparam name="TState">TState.</typeparam>
        /// <param name="logLevel">logLevel.</param>
        /// <param name="eventId">eventId.</param>
        /// <param name="state">state.</param>
        /// <param name="exception">exception.</param>
        /// <param name="formatter">formatter.</param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter != null)
            {
                lock (_lock)
                {
                    File.AppendAllText(this.filePath, formatter(state, exception) + Environment.NewLine);
                }
            }
        }
    }
}
