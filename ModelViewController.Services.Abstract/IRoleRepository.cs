// <copyright file="IRoleRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ModelViewController.Services.Abstract
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// IRoleRepository interface.
    /// </summary>
    /// <typeparam name="T">item.</typeparam>
    public interface IRoleRepository<T>
    {
        /// <summary>
        /// Add method.
        /// </summary>
        /// <param name="item">item.</param>
        /// <returns>task item.</returns>
        Task<T> Add(T item);

        /// <summary>
        /// Remove method.
        /// </summary>
        /// <param name="id">id.</param>
        /// <returns>task.</returns>
        Task Remove(Guid id);

        /// <summary>
        /// Update method.
        /// </summary>
        /// <param name="item">item.</param>
        /// <returns>task.</returns>
        Task Update(T item);

        /// <summary>
        /// GetAll method.
        /// </summary>
        /// <returns>item.</returns>
        List<T> GetAll();

        /// <summary>
        /// GetAllAsync method.
        /// </summary>
        /// <returns>list item.</returns>
        Task<List<T>> GetAllAsync();

        /// <summary>
        /// Find method.
        /// </summary>
        /// <param name="id">id.</param>
        /// <returns>item.</returns>
        Task<T> Find(Guid? id);
    }
}
