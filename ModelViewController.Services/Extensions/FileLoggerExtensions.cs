// <copyright file="FileLoggerExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ModelViewController.Services.Extensions
{
    using Microsoft.Extensions.Logging;
    using ModelViewController.Services.Providers;

    /// <summary>
    /// FileLoggerExtensions static class.
    /// </summary>
    public static class FileLoggerExtensions
    {
        /// <summary>
        /// AddFile mehod.
        /// </summary>
        /// <param name="factory">factory.</param>
        /// <param name="filePath">filePath.</param>
        /// <returns>ILoggerFactory.</returns>
        public static ILoggerFactory AddFile(
            this ILoggerFactory factory, string filePath)
        {
            factory.AddProvider(new FileLoggerProvider(filePath));
            return factory;
        }
    }
}
