// <copyright file="AccountController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ModelViewController.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using ModelViewController.DAL;
    using ModelViewController.DAL.Entities;
    using ModelViewController.Models;
    using ModelViewController.Services.Abstract;
    using SmartBreadcrumbs.Attributes;

    /// <summary>
    /// AccountController.
    /// </summary>
    [Breadcrumb("Account")]
    public class AccountController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IAwardRepository<User> _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="context">context.</param>
        /// <param name="repository">repository.</param>
        public AccountController(ApplicationContext context, IAwardRepository<User> repository)
        {
            this._context = context;
            this._repository = repository;
        }

        /// <summary>
        /// Index method.
        /// </summary>
        /// <returns>view.</returns>
        public IActionResult Index()
        {
            return this.View();
        }

        /// <summary>
        /// Register method.
        /// </summary>
        /// <returns>View.</returns>
        [HttpGet]
        [Breadcrumb("Register")]
        public IActionResult Register()
        {
            return this.View();
        }

        /// <summary>
        /// .
        /// </summary>
        /// <param name="model">model.</param>
        /// <returns>View.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Breadcrumb("Register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (this.ModelState.IsValid)
            {
                User user = await this._context.Users.FirstOrDefaultAsync(u => u.UserName == model.Login);
                var role = this._context.Roles.FirstOrDefault(u => u.Name.Equals("user"));

                if (user == null)
                {
                    var newUser = new User
                        {
                            UserName = model.Login,
                            Password = model.Password,
                            Name = model.Name,
                            Birthdate = model.Birthdate,
                        };
                    this._context.Users.Add(newUser);
                    await this._context.SaveChangesAsync();

                    // add Photo
                    if (model.Photo != null)
                    {
                        await this._repository.AddFile(user, model.Photo);
                    }

                    // add default role
                    var usr = await this._context.Users.FirstOrDefaultAsync(u => u.UserName == newUser.UserName);
                    this._context.Add(new UserRole { UserId = usr.Id, RoleId = role.Id });
                    await this._context.SaveChangesAsync();

                    // authentication
                    await this.Authenticate(usr);

                    return this.RedirectToAction("Index", "Home");
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "Incorrect username and/or password");
                }
            }

            return this.View(model);
        }

        /// <summary>
        /// .
        /// </summary>
        /// <returns>View.</returns>
        [HttpGet]
        [Breadcrumb("Login")]
        public IActionResult Login()
        {
            return this.View();
        }

        /// <summary>
        /// .
        /// </summary>
        /// <param name="model">model.</param>
        /// <returns>View.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Breadcrumb("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (this.ModelState.IsValid)
            {
                User user = await this._context.Users
                    .Include(u => u.UserRoles)
                    .ThenInclude(ua => ua.Role)
                    .FirstOrDefaultAsync(u => u.UserName == model.Login
                        && u.Password == model.Password);
                if (user != null)
                {
                    await this.Authenticate(user);
                    return this.RedirectToAction("Index", "Home");
                }

                this.ModelState.AddModelError(string.Empty, "Incorrect username and/or password");
            }

            return this.View(model);
        }

        /// <summary>
        /// LogOff.
        /// </summary>
        /// <returns>View.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await this.HttpContext.SignOutAsync();
            return this.RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Authenticate.
        /// </summary>
        /// <param name="user">user.</param>
        /// <returns>Task.</returns>
        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
            };
            foreach (var userRole in user.UserRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole.Role.Name));
            }

            ClaimsIdentity id = new ClaimsIdentity(
                    claims,
                    "ApplicationCookie",
                    ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);

            await this.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}