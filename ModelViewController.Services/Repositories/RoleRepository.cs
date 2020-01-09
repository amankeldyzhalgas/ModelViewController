// <copyright file="RoleRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ModelViewController.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using ModelViewController.DAL;
    using ModelViewController.DAL.Entities;
    using ModelViewController.Services.Abstract;

    /// <summary>
    /// RoleRepository class.
    /// </summary>
    public class RoleRepository : IRoleRepository<Role>
    {
        private readonly ApplicationContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleRepository"/> class.
        /// </summary>
        /// <param name="context">context.</param>
        public RoleRepository(ApplicationContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// Add method.
        /// </summary>
        /// <param name="role">item.</param>
        /// <returns>added item.</returns>
        public async Task<Role> Add(Role role)
        {
            role.Id = Guid.NewGuid();
            this._context.Add(role);
            await this._context.SaveChangesAsync();
            return role;
        }

        /// <summary>
        /// Find method.
        /// </summary>
        /// <param name="id">id.</param>
        /// <returns>item.</returns>
        public async Task<Role> Find(Guid? id)
        {
            return await this._context.Roles.Include(u => u.UserRoles).ThenInclude(ua => ua.User).Where(u => u.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// GetAll method.
        /// </summary>
        /// <returns>roles.</returns>
        public List<Role> GetAll()
        {
            return this._context.Roles.Include(u => u.UserRoles).ThenInclude(ua => ua.User).ToList();
        }

        /// <summary>
        /// GetAllAsync method.
        /// </summary>
        /// <returns>roles.</returns>
        public async Task<List<Role>> GetAllAsync()
        {
            return await this._context.Roles.Include(u => u.UserRoles).ThenInclude(ua => ua.User).ToListAsync();
        }

        /// <summary>
        /// Remove method.
        /// </summary>
        /// <param name="id">id.</param>
        /// <returns>task.</returns>
        public async Task Remove(Guid id)
        {
            var user = await this.Find(id);
            this._context.Remove(user);
            await this._context.SaveChangesAsync();
        }

        /// <summary>
        /// Update method.
        /// </summary>
        /// <param name="role">item.</param>
        /// <returns>task.</returns>
        public async Task Update(Role role)
        {
            this._context.Update(role);
            await this._context.SaveChangesAsync();
        }
    }
}
