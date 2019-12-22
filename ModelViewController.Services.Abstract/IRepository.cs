// <copyright file="IRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ModelViewController.Services.Abstract
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// IRepository interface.
    /// </summary>
    /// <typeparam name="T">Object.</typeparam>
    public interface IRepository<T>
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
        /// Filter method.
        /// </summary>
        /// <param name="param">param.</param>
        /// <returns>list item.</returns>
        Task<List<T>> Filter(string param);

        /// <summary>
        /// Find method.
        /// </summary>
        /// <param name="id">id.</param>
        /// <returns>item.</returns>
        Task<T> Find(Guid? id);

        /// <summary>
        /// AddFile method.
        /// </summary>
        /// <param name="item">item.</param>
        /// <param name="uploadedFile">IFormFile.</param>
        /// <returns>task.</returns>
        Task AddFile(T item, IFormFile uploadedFile);

        /// <summary>
        /// RemoveFile method.
        /// </summary>
        /// <param name="path">path.</param>
        void RemoveFile(string path);

        /// <summary>
        /// UpdateUserAwards method.
        /// </summary>
        /// <param name="item">item.</param>
        /// <param name="list">list.</param>
        /// <returns>task.</returns>
        Task UpdateUserAwards(T item, List<Guid> list);
    }
}
