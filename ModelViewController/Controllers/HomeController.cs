// <copyright file="HomeController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ModelViewController.Controllers
{
    using System.Diagnostics;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using ModelViewController.Models;
    using SmartBreadcrumbs.Attributes;

    /// <summary>
    /// HomeController.
    /// </summary>
    [Breadcrumb("Home")]
    public class HomeController : Controller
    {
        private readonly IMemoryCache _cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="cache">cache.</param>
        public HomeController(IMemoryCache cache)
        {
            this._cache = cache;
        }

        /// <summary>
        /// Index method.
        /// </summary>
        /// <returns>View.</returns>
        public IActionResult Index()
        {
            if (this.User.IsInRole("candidate"))
            {
                this._cache.TryGetValue(this.User.Identity.Name, out CacheModel changes);
                if (changes != null)
                {
                    this.ViewBag.CacheDatas = changes;
                    this.ViewBag.CacheIsEmpty = false;
                }
                else
                {
                    this.ViewBag.CacheIsEmpty = true;
                }
            }

            return this.View();
        }

        /// <summary>
        /// Privacy method.
        /// </summary>
        /// <returns>View.</returns>
        public IActionResult Privacy()
        {
            return this.View();
        }

        /// <summary>
        /// Error method.
        /// </summary>
        /// <returns>Error View.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
