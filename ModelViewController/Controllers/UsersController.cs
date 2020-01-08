// <copyright file="UsersController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ModelViewController.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using ModelViewController.DAL.Entities;
    using ModelViewController.Models;
    using ModelViewController.Models.Users;
    using ModelViewController.Services;
    using ModelViewController.Services.Abstract;
    using SmartBreadcrumbs.Attributes;

    /// <summary>
    /// Users Controller.
    /// </summary>
    [Authorize]
    [Breadcrumb("Users")]
    public class UsersController : Controller
    {
        private readonly IUserRepository<User> _userRepository;
        private readonly IAwardRepository<Award> _awardRepository;
        private readonly IRoleRepository<Role> _roleRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="userRepository">User repository.</param>
        /// <param name="awardRepository">Award repository.</param>
        /// <param name="roleRepository">Role repository.</param>
        public UsersController(IUserRepository<User> userRepository, IAwardRepository<Award> awardRepository, IRoleRepository<Role> roleRepository)
        {
            this._userRepository = userRepository;
            this._awardRepository = awardRepository;
            this._roleRepository = roleRepository;
        }

        // GET: Users

        /// <summary>
        /// Index method.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [AllowAnonymous]
        [Route("/users")]
        public async Task<IActionResult> Index()
        {
            return this.View(await this._userRepository.GetAllAsync());
        }

        /// <summary>
        /// Index method with param.
        /// </summary>
        /// <param name="name">name.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Route("/users/{name}")]
        public async Task<IActionResult> Index(string name)
        {
            return this.View(await this._userRepository.Filter(name));
        }

        /// <summary>
        /// .
        /// </summary>
        /// <param name="id">User Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        // GET: Users/Details/5
        [Breadcrumb("Details")]
        [Route("user/{id}")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            if (Guid.TryParse(id, out Guid guid))
            {
                var user = await this._userRepository.Find(guid);
                return this.View(user);
            }

            var users = await this._userRepository.Filter(id);
            if (users != null)
            {
                var user = users.First();
                return this.View(user);
            }

            return this.NotFound();
        }

        // GET: Users/Create

        /// <summary>
        /// .
        /// </summary>
        /// <returns>Create View.</returns>
        [Breadcrumb("Create")]
        [Authorize(Roles = "admin")]
        [Route("/create-user")]
        public IActionResult Create()
        {
            this.ViewData["Awards"] = this._awardRepository.GetAll();
            return this.View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        /// <summary>
        /// .
        /// </summary>
        /// <param name="model">UserModel.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Breadcrumb("Create")]
        [Authorize(Roles = "admin")]
        [Route("/create-user")]
        public async Task<IActionResult> Create(UserModel model)
        {
            if (this.ModelState.IsValid)
            {
                if (model.Birthdate > DateTime.Now || DateTime.Now.Year - 150 > model.Birthdate.Year)
                {
                    this.ModelState.AddModelError(string.Empty, "The age of the user can not be negative, can not be more than 150 years.");

                    this.ViewData["Awards"] = this._awardRepository.GetAll();
                    return this.View(model);
                }

                var user = new User { Name = model.Name, Birthdate = model.Birthdate };
                user = await this._userRepository.Add(user);

                if (model.Awards != null)
                {
                    await this._userRepository.UpdateUserAwards(user, model.Awards);
                }

                if (model.Photo != null)
                {
                    await this._userRepository.AddFile(user, model.Photo);
                }

                return this.RedirectToAction(nameof(this.Index));
            }

            this.ViewData["Awards"] = this._awardRepository.GetAll();
            return this.View(model);
        }

        /// <summary>
        /// .
        /// </summary>
        /// <param name="id">id.</param>
        /// <returns>View.</returns>
        [Breadcrumb("AddAwards")]
        public async Task<IActionResult> AddAwards(Guid? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var user = await this._userRepository.Find(id);
            var userAwards = await this._userRepository.GetUserAwards(id);
            var awards = await this._awardRepository.GetAllAsync();
            var model = new AddAwardModel { UserId = user.Id, Awards = awards, UserAwards = userAwards };
            if (user == null)
            {
                return this.NotFound();
            }

            return this.View(model);
        }

        /// <summary>
        /// .
        /// </summary>
        /// <param name="id">id.</param>
        /// <param name="model">model.</param>
        /// <returns>View.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Breadcrumb("AddAwards")]
        public async Task<IActionResult> AddAwards(Guid id, AddAwardModel model)
        {
            if (id != model.UserId)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    var user = await this._userRepository.Find(id);
                    if (model.Awards != null)
                    {
                        await this._userRepository.UpdateUserAwards(user, model.Awards);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.UserExists(model.UserId))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return this.RedirectToAction(nameof(this.Index));
            }

            this.ViewData["Awards"] = this._awardRepository.GetAll();
            return this.View(model);
        }

        /// <summary>
        /// AddAwards.
        /// </summary>
        /// <param name="id">id.</param>
        /// <param name="awardId">awardId.</param>
        /// <returns>awards.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/award-user/{user}_{award}")]
        public async Task<IActionResult> AddAwards(Guid id, Guid awardId)
        {
            try
            {
                var user = await this._userRepository.Find(id);
                var award = await this._awardRepository.Find(awardId);
                if (award != null)
                {
                    var awards = new List<Award>();
                    awards.Add(award);
                    await this._userRepository.UpdateUserAwards(user, awards);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.UserExists(id))
                {
                    return this.NotFound();
                }
                else
                {
                    throw;
                }
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        /// <summary>
        /// .
        /// </summary>
        /// <param name="id">User Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        // GET: Users/Edit/5
        [Breadcrumb("Edit")]
        [Authorize(Roles = "admin")]
        [Route("/user/{id}/edit")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var user = await this._userRepository.Find(id);
            var userAwards = await this._userRepository.GetUserAwards(id);
            var awards = await this._awardRepository.GetAllAsync();
            var model = new UserModel { Id = user.Id, Name = user.Name, Birthdate = user.Birthdate, PhotoSrc = user.Photo, UserAwards = userAwards, Awards = awards };
            if (user == null)
            {
                return this.NotFound();
            }

            return this.View(model);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        /// <summary>
        /// .
        /// </summary>
        /// <param name="id">User Id.</param>
        /// <param name="model">Model.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Breadcrumb("Edit")]
        [Authorize(Roles = "admin")]
        [Route("/user/{id}/edit")]
        public async Task<IActionResult> Edit(Guid id, UserModel model)
        {
            if (id != model.Id)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    var user = await this._userRepository.Find(id);
                    user.Name = model.Name;
                    user.Birthdate = model.Birthdate;
                    if (model.Awards != null)
                    {
                        await this._userRepository.UpdateUserAwards(user, model.Awards);
                    }

                    if (model.Photo != null)
                    {
                        await this._userRepository.AddFile(user, model.Photo);
                    }

                    await this._userRepository.Update(user);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.UserExists(model.Id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return this.RedirectToAction(nameof(this.Index));
            }

            this.ViewData["Awards"] = this._awardRepository.GetAll();
            return this.View(model);
        }

        /// <summary>
        /// ChangeRole.
        /// </summary>
        /// <param name="id">id.</param>
        /// <returns>View.</returns>
        [Breadcrumb("ChangeRole")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangeRole(Guid? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var user = await this._userRepository.Find(id);
            var userRoles = await this._userRepository.GetUserRoles(id);
            var roles = await this._roleRepository.GetAllAsync();
            var model = new ChangeUserRolesModel { UserId = user.Id, UserName = user.UserName,  UserRoles = userRoles, Roles = roles };
            if (user == null)
            {
                return this.NotFound();
            }

            return this.View(model);
        }

        // GET: Users/Delete/5

        /// <summary>
        /// .
        /// </summary>
        /// <param name="id">User Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [Breadcrumb("Delete")]
        [Authorize(Roles = "admin")]
        [Route("/user/{id}/delete")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var user = await this._userRepository.Find(id);
            if (user == null)
            {
                return this.NotFound();
            }

            return this.View(user);
        }

        // POST: Users/Delete/5

        /// <summary>
        /// .
        /// </summary>
        /// <param name="id">User Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost]
        [ActionName("Delete")]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await this._userRepository.Remove(id);
            return this.RedirectToAction(nameof(this.Index));
        }

        /// <summary>
        /// .
        /// </summary>
        /// <returns>Physical File.</returns>
        public IActionResult Download()
        {
            var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "wwwroot", "users.txt");
            using (var stream = new FileStream(path, FileMode.OpenOrCreate))
            {
                foreach (var item in this._userRepository.GetAll())
                {
                    var str = $"Name: {item.Name} Birthdate: {item.Birthdate} Photo: {item.Photo}{Environment.NewLine}Awards{Environment.NewLine}";
                    if (item.UserAwards != null)
                    {
                        foreach (var award in item.UserAwards)
                        {
                            str += $"Title: {award.Award.Title} ";
                        }

                        str += $"{Environment.NewLine}";
                    }
                    else
                    {
                        str += $"Empty.{Environment.NewLine}";
                    }

                    byte[] array = System.Text.Encoding.Default.GetBytes(str);
                    stream.Write(array, 0, array.Length);
                }
            }

            return this.PhysicalFile(path, "text/plain", "users.txt");
        }

        private bool UserExists(Guid id)
        {
            return this._userRepository.GetAll().Any(e => e.Id == id);
        }
    }
}
