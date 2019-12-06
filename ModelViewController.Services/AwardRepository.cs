// <copyright file="AwardRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ModelViewController.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using ModelViewController.DAL;
    using ModelViewController.DAL.Entities;
    using ModelViewController.Services.Abstract;

    /// <summary>
    /// .
    /// </summary>
    public class AwardRepository : IRepository<Award>
    {
        private readonly ApplicationContext _context;
        private readonly IHostingEnvironment _appEnvironment;

        /// <summary>
        /// Initializes a new instance of the <see cref="AwardRepository"/> class.
        /// </summary>
        /// <param name="context">ApplicationContext.</param>
        /// <param name="appEnvironment">IHostingEnvironment.</param>
        public AwardRepository(ApplicationContext context, IHostingEnvironment appEnvironment)
        {
            this._context = context;
            this._appEnvironment = appEnvironment;
        }

        /// <summary>
        /// Add method.
        /// </summary>
        /// <param name="award">Award.</param>
        /// <returns>award.</returns>
        public async Task<Award> Add(Award award)
        {
            award.Id = Guid.NewGuid();
            this._context.Add(award);
            await this._context.SaveChangesAsync();
            return award;
        }

        /// <summary>
        /// Find method.
        /// </summary>
        /// <param name="id">id.</param>
        /// <returns>award.</returns>
        public async Task<Award> Find(Guid? id)
        {
            return await this._context.Awards.FindAsync(id);
        }

        /// <summary>
        /// GetAll method.
        /// </summary>
        /// <returns>awards.</returns>
        public List<Award> GetAll()
        {
            return this._context.Awards.ToList();
        }

        /// <summary>
        /// GetAllAsync method.
        /// </summary>
        /// <returns>awards.</returns>
        public async Task<List<Award>> GetAllAsync()
        {
            return await this._context.Awards.ToListAsync();
        }

        /// <summary>
        /// Remove method.
        /// </summary>
        /// <param name="id">id.</param>
        /// <returns>Task.</returns>
        public async Task Remove(Guid id)
        {
            var award = await this.Find(id);
            this.RemoveFile(award.Image);
            this._context.Awards.Remove(award);
            await this._context.SaveChangesAsync();
        }

        /// <summary>
        /// Update method.
        /// </summary>
        /// <param name="award">award.</param>
        /// <returns>Task.</returns>
        public async Task Update(Award award)
        {
            this._context.Update(award);
            await this._context.SaveChangesAsync();
        }

        /// <summary>
        /// AddFile method.
        /// </summary>
        /// <param name="award">awawrd.</param>
        /// <param name="uploadedFile">IFormFile.</param>
        /// <returns>Task.</returns>
        public async Task AddFile(Award award, IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                string path = "/img/awards/" + uploadedFile.FileName;

                using (var fileStream = new FileStream(this._appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }

                award.Image = path;
                await this.Update(award);
            }
        }

        /// <summary>
        /// RemoveFile method.
        /// </summary>
        /// <param name="path">path.</param>
        public void RemoveFile(string path)
        {
            if (!string.IsNullOrEmpty(path) && !this.GetAll().Any(a => a.Image.Equals(path)))
            {
                var fileInfo = new FileInfo(this._appEnvironment.WebRootPath + path);
                fileInfo.Delete();
            }
        }

        /// <summary>
        /// NotImplemented.
        /// </summary>
        /// <param name="item">Award.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns><param name="list">Guid list.</param>
        public Task UpdateUserAwards(Award item, List<Guid> list)
        {
            throw new NotImplementedException();
        }
    }
}
