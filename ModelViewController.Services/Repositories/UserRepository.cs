// <copyright file="UserRepository.cs" company="PlaceholderCompany">
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
    /// UserRepository.
    /// </summary>
    public class UserRepository : IUserRepository<User>
    {
        private readonly ApplicationContext _context;
        private readonly IHostingEnvironment _appEnvironment;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="context">ApplicationContext.</param>
        /// <param name="appEnvironment">IHostingEnvironment.</param>
        public UserRepository(ApplicationContext context, IHostingEnvironment appEnvironment)
        {
            this._context = context;
            this._appEnvironment = appEnvironment;
        }

        /// <summary>
        /// Add method.
        /// </summary>
        /// <param name="user">User.</param>
        /// <returns>user.</returns>
        public async Task<User> Add(User user)
        {
            user.Id = Guid.NewGuid();
            this._context.Add(user);
            await this._context.SaveChangesAsync();
            return user;
        }

        /// <summary>
        /// Find method.
        /// </summary>
        /// <param name="id">id.</param>
        /// <returns>user.</returns>
        public async Task<User> Find(Guid? id)
        {
            return await this._context.Users
                .Include(u => u.UserAwards)
                .ThenInclude(ua => ua.Award)
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// GetAll method.
        /// </summary>
        /// <returns>users.</returns>
        public List<User> GetAll()
        {
            return this._context.Users.Include(u => u.UserAwards).ThenInclude(ua => ua.Award).ToList();
        }

        /// <summary>
        /// GetAllAsync method.
        /// </summary>
        /// <returns>users.</returns>
        public async Task<List<User>> GetAllAsync()
        {
            return await this._context.Users.Include(u => u.UserAwards).ThenInclude(ua => ua.Award).ToListAsync();
        }

        /// <summary>
        /// Filter method.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <returns>users.</returns>
        public async Task<List<User>> Filter(string name)
        {
            var users = await this.GetAllAsync();
            if (name.Length == 1)
            {
                return users.Where(u => u.Name.StartsWith(name)).ToList();
            }
            else if (name.Contains("_"))
            {
                name = name.Replace("_", " ");
                return users.Where(u => u.Name.Equals(name)).OrderBy(u => u.Birthdate).Take(1).ToList();
            }
            else
            {
                return users.Where(u => u.Name.Contains(name)).ToList();
            }
        }

        /// <summary>
        /// GetUserRoles.
        /// </summary>
        /// <param name="id">id.</param>
        /// <returns>roles.</returns>
        public async Task<List<Role>> GetUserRoles(Guid? id)
        {
            var user = await this.Find(id);
            var roles = user.UserRoles.Select(ur => ur.Role).ToList();
            return roles;
        }

        /// <summary>
        /// GetUserAwards.
        /// </summary>
        /// <param name="id">id.</param>
        /// <returns>awards.</returns>
        public async Task<List<Award>> GetUserAwards(Guid? id)
        {
            var user = await this.Find(id);
            var awards = user.UserAwards.Select(ur => ur.Award).ToList();
            return awards;
        }

        /// <summary>
        /// GetAllAsync method.
        /// </summary>
        /// <param name="param">param.</param>
        /// <returns>user.</returns>
        /*public async Task<List<User>> Filter(string param)
        {
            return await this._context.Users
                .Include(u => u.UserAwards)
                .ThenInclude(ua => ua.Award)
                .Where(u => u.Name.StartsWith(param))
                .ToListAsync();
        }*/

        /// <summary>
        /// Remove method.
        /// </summary>
        /// <param name="id">id.</param>
        /// <returns>task.</returns>
        public async Task Remove(Guid id)
        {
            var user = await this.Find(id);
            this.RemoveFile(user.Photo);
            this._context.Remove(user);
            await this._context.SaveChangesAsync();
        }

        /// <summary>
        /// Update method.
        /// </summary>
        /// <param name="user">User.</param>
        /// <returns>task.</returns>
        public async Task Update(User user)
        {
            this._context.Update(user);
            await this._context.SaveChangesAsync();
        }

        /// <summary>
        /// AddFile method.
        /// </summary>
        /// <param name="user">User.</param>
        /// <param name="uploadedFile">IFormFile.</param>
        /// <returns>task.</returns>
        public async Task AddFile(User user, IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                string path = "/img/users/" + uploadedFile.FileName;

                using (var fileStream = new FileStream(this._appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }

                user.Photo = path;
                await this.Update(user);
            }
        }

        /// <summary>
        /// AddAwards method.
        /// </summary>
        /// <param name="user">User.</param>
        /// <param name="awards">Awards.</param>
        /// <returns>task.</returns>
        public async Task AddAwards(User user, List<Award> awards)
        {
            foreach (var award in awards)
            {
               user.UserAwards.Add(new UserAward { AwardId = award.Id, UserId = user.Id });
            }

            await this._context.SaveChangesAsync();
        }

        /// <summary>
        /// RemoveFile method.
        /// </summary>
        /// <param name="path">path.</param>
        public void RemoveFile(string path)
        {
            if (!string.IsNullOrEmpty(path) && this.GetAll().Any(u => u.Photo.Equals(path)))
            {
                var fileInfo = new FileInfo(this._appEnvironment.WebRootPath + path);
                fileInfo.Delete();
            }
        }

        /// <summary>
        /// UpdateUserAwards method.
        /// </summary>
        /// <param name="user">User.</param>
        /// <param name="awards">Awards.</param>
        /// <returns>task.</returns>
        public async Task UpdateUserAwards(User user, List<Award> awards)
        {
            user.UserAwards.Clear();
            if (awards != null)
            {
                foreach (var award in awards)
                {
                    user.UserAwards.Add(new UserAward { UserId = user.Id, AwardId = award.Id });
                }
            }

            await this.Update(user);
        }
    }
}
