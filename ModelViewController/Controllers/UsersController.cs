// <copyright file="UsersController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ModelViewController.Controllers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using ModelViewController.DAL.Entities;
    using ModelViewController.Models;
    using ModelViewController.Services.Abstract;
    using SmartBreadcrumbs.Attributes;

    /// <summary>
    /// Users Controller.
    /// </summary>
    [Breadcrumb("Users")]
    public class UsersController : Controller
    {
        private readonly IRepository<User> _repository;
        private readonly IRepository<Award> _awardRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="repository">User repository.</param>
        /// <param name="awardRepository">Award repository.</param>
        public UsersController(IRepository<User> repository, IRepository<Award> awardRepository)
        {
            this._repository = repository;
            this._awardRepository = awardRepository;
        }

        // GET: Users

        /// <summary>
        /// .
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<IActionResult> Index()
        {
            return this.View(await this._repository.GetAllAsync());
        }

        /*
        /// <summary>
        /// .
        /// </summary>
        /// <param name="name">name.</param>
        /// <returns>users.</returns>
        public async Task<IActionResult> Filter(string name)
        {
            return this.View(await this._repository.Filter(name));
        }*/

        // GET: Users/Details/5

        /// <summary>
        /// .
        /// </summary>
        /// <param name="id">User Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [Breadcrumb("Details")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var user = await this._repository.Find(id);
            if (user == null)
            {
                return this.NotFound();
            }

            return this.View(user);
        }

        // GET: Users/Create

        /// <summary>
        /// .
        /// </summary>
        /// <returns>Create View.</returns>
        [Breadcrumb("Create")]
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
                user = await this._repository.Add(user);

                if (model.Awards != null)
                {
                    await this._repository.UpdateUserAwards(user, model.Awards);
                }

                if (model.Photo != null)
                {
                    await this._repository.AddFile(user, model.Photo);
                }

                return this.RedirectToAction(nameof(this.Index));
            }

            this.ViewData["Awards"] = this._awardRepository.GetAll();
            return this.View(model);
        }

        // GET: Users/Edit/5

        /// <summary>
        /// .
        /// </summary>
        /// <param name="id">User Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [Breadcrumb("Edit")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var user = await this._repository.Find(id);
            var model = new UserModel { Id = user.Id, Name = user.Name, Birthdate = user.Birthdate, PhotoSrc = user.Photo, Awards = user.UserAwards.Select(ua => ua.AwardId).ToList() };
            if (user == null)
            {
                return this.NotFound();
            }

            this.ViewData["Awards"] = this._awardRepository.GetAll();
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
                    var user = await this._repository.Find(id);
                    user.Name = model.Name;
                    user.Birthdate = model.Birthdate;
                    if (model.Awards != null)
                    {
                        await this._repository.UpdateUserAwards(user, model.Awards);
                    }

                    if (model.Photo != null)
                    {
                        await this._repository.AddFile(user, model.Photo);
                    }
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

        // GET: Users/Delete/5

        /// <summary>
        /// .
        /// </summary>
        /// <param name="id">User Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [Breadcrumb("Delete")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var user = await this._repository.Find(id);
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await this._repository.Remove(id);
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
                foreach (var item in this._repository.GetAll())
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
            return this._repository.GetAll().Any(e => e.Id == id);
        }
    }
}
