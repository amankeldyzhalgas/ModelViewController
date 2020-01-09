// <copyright file="FileLoggerProvider.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ModelViewController.Services.Providers
{
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// FileLoggerProvider class.
    /// </summary>
    public class FileLoggerProvider : ILoggerProvider
    {
        private readonly string path;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileLoggerProvider"/> class.
        /// </summary>
        /// <param name="_path">path.</param>
        public FileLoggerProvider(string _path)
        {
            this.path = _path;
        }

        /// <summary>
        /// CreateLogger method.
        /// </summary>
        /// <param name="categoryName">categoryName.</param>
        /// <returns>ILogger.</returns>
        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(this.path);
        }

        /// <summary>
        /// Dispose method.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
