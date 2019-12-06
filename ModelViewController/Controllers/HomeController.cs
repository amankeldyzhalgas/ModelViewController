// <copyright file="HomeController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ModelViewController.Controllers
{
    using System.Diagnostics;
    using Microsoft.AspNetCore.Mvc;
    using ModelViewController.Models;

    /// <summary>
    /// .
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// .
        /// </summary>
        /// <returns>View.</returns>
        public IActionResult Index()
        {
            return this.View();
        }

        /// <summary>
        /// .
        /// </summary>
        /// <returns>View.</returns>
        public IActionResult Privacy()
        {
            return this.View();
        }

        /// <summary>
        /// .
        /// </summary>
        /// <returns>Error View.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
