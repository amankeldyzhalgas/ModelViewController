// <copyright file="AwardsController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ModelViewController.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using ModelViewController.DAL.Entities;
    using ModelViewController.Models;
    using ModelViewController.Services.Abstract;
    using SmartBreadcrumbs.Attributes;

    /// <summary>
    /// Awards Controller.
    /// </summary>
    [Breadcrumb("Awards")]
    public class AwardsController : Controller
    {
        private readonly IRepository<Award> _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AwardsController"/> class.
        /// </summary>
        /// <param name="repository">Repository.</param>
        public AwardsController(IRepository<Award> repository)
        {
            this._repository = repository;
        }

        // GET: Awards

        /// <summary>
        /// Index.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<IActionResult> Index()
        {
            return this.View(await this._repository.GetAllAsync());
        }

        // GET: Awards/Details/5

        /// <summary>
        /// Details.
        /// </summary>
        /// <param name="id">Award Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [Breadcrumb("Details")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var award = await this._repository.Find(id);
            if (award == null)
            {
                return this.NotFound();
            }

            return this.View(award);
        }

        // GET: Awards/Create

        /// <summary>
        /// .
        /// </summary>
        /// <returns>Create View.</returns>
        [Breadcrumb("Create")]
        public IActionResult Create()
        {
            return this.View();
        }

        // POST: Awards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        /// <summary>
        /// Create.
        /// </summary>
        /// <param name="model">AwardModel.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AwardModel model)
        {
            if (this.ModelState.IsValid)
            {
                var award = new Award { Title = model.Title, Description = model.Description };
                await this._repository.Add(award);
                if (model.Image != null)
                {
                    await this._repository.AddFile(award, model.Image);
                }

                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(model);
        }

        // GET: Awards/Edit/5

        /// <summary>
        /// .
        /// </summary>
        /// <param name="id">Award Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [Breadcrumb("Edit")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var award = await this._repository.Find(id);
            if (award == null)
            {
                return this.NotFound();
            }

            return this.View(award);
        }

        // POST: Awards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        /// <summary>
        /// .
        /// </summary>
        /// <param name="id">Award Id.</param>
        /// <param name="award">Award.</param>
        /// <param name="image">Image.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,Description")] Award award, IFormFile image)
        {
            if (id != award.Id)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    await this._repository.Update(award);
                    if (image != null)
                    {
                        await this._repository.AddFile(award, image);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.AwardExists(award.Id))
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

            return this.View(award);
        }

        // GET: Awards/Delete/5

        /// <summary>
        /// .
        /// </summary>
        /// <param name="id">Award Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [Breadcrumb("Delete")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var award = await this._repository.Find(id);
            if (award == null)
            {
                return this.NotFound();
            }

            return this.View(award);
        }

        // POST: Awards/Delete/5

        /// <summary>
        /// .
        /// </summary>
        /// <param name="id">Award Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await this._repository.Remove(id);
            return this.RedirectToAction(nameof(this.Index));
        }

        private bool AwardExists(Guid id)
        {
            return this._repository.GetAll().Any(e => e.Id == id);
        }
    }
}
