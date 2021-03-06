﻿// <copyright file="Startup.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ModelViewController
{
    using System;
    using System.IO;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using ModelViewController.DAL;
    using ModelViewController.DAL.Entities;
    using ModelViewController.Services;
    using ModelViewController.Services.Abstract;
    using ModelViewController.Services.Extensions;
    using SmartBreadcrumbs.Extensions;

    /// <summary>
    /// Startup class.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">configuration.</param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// Gets Configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.

        /// <summary>
        /// ConfigureServices method.
        /// </summary>
        /// <param name="services">services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            string connection = @"Server=(localdb)\mssqllocaldb;Database=dbMVC;Trusted_Connection=True;";
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));
            services.AddScoped<IAwardRepository<Award>, AwardRepository>();
            services.AddScoped<IUserRepository<User>, UserRepository>();
            services.AddScoped<IRoleRepository<Role>, RoleRepository>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                            {
                                options.LoginPath = new PathString("/Account/Login");
                                options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddBreadcrumbs(this.GetType().Assembly, options =>
            {
                // Testing
                options.DontLookForDefaultNode = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

        /// <summary>
        /// Configure.
        /// </summary>
        /// <param name="app">IApplicationBuilder.</param>
        /// <param name="env">IHostingEnvironment.</param>
        /// <param name="log">ILoggerFactory.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory log)
        {
            var date = DateTime.Now.ToShortDateString();
            log.AddFile(Path.Combine(Directory.GetCurrentDirectory() + "/Logs/", $"log_{date}.txt"));
            var logger = log.CreateLogger("FileLogger");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}/{otherId?}");
            });
        }
    }
}
