using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ModelViewController.DAL;
using ModelViewController.DAL.Entities;
using ModelViewController.Services.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ModelViewController.Services
{
    public class UserRepository : IRepository<User>
    {
        private readonly ApplicationContext _context;
        private readonly IHostingEnvironment _appEnvironment;

        public UserRepository(ApplicationContext context, IHostingEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }
        public async Task Add(User user)
        {
            user.Id = Guid.NewGuid();
            _context.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> Find(Guid? id)
        {
            return await _context.Users.FindAsync(id);
        }

        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task Remove(Guid id)
        {
            var user = await Find(id);
            RemoveFile(user.Photo);
            _context.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task Update(User user)
        {
            _context.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task AddFile(User user, IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                string path = "/img/users/" + uploadedFile.FileName;

                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }

                user.Photo = path;
                await Update(user);
            }
        }

        public void RemoveFile(string path)
        {
            if (!string.IsNullOrEmpty(path) && GetAll().Any(u => u.Photo.Equals(path)))
            {
                var fileInfo = new FileInfo(_appEnvironment.WebRootPath + path);
                fileInfo.Delete();
            }
        }
    }
}
