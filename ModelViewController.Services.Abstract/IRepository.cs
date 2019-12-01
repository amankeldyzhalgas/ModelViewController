using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ModelViewController.Services.Abstract
{
    public interface IRepository<T>
    {
        Task Add(T item);
        Task Remove(Guid id);
        Task Update(T item);
        List<T> GetAll();
        Task<List<T>> GetAllAsync();
        Task<T> Find(Guid? id);
        Task AddFile(T item, IFormFile uploadedFile);
        void RemoveFile(string path);
    }
}
