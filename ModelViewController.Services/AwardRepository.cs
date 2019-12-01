using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
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
    public class AwardRepository : IRepository<Award>
    {
        private readonly ApplicationContext _context;
        private readonly IHostingEnvironment _appEnvironment;

        public AwardRepository(ApplicationContext context, IHostingEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public async Task Add(Award award)
        {
            award.Id = Guid.NewGuid();
            _context.Add(award);
            await _context.SaveChangesAsync();
        }

        public async Task<Award> Find(Guid? id)
        {
            return await _context.Awards.FindAsync(id);
        }

        public List<Award> GetAll()
        {
            return _context.Awards.ToList();
        }

        public async Task<List<Award>> GetAllAsync()
        {
            return await _context.Awards.ToListAsync();
        }

        public async Task Remove(Guid id)
        {
            var award = await Find(id);
            RemoveFile(award.Image);
            _context.Awards.Remove(award);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Award award)
        {
            _context.Update(award);
            await _context.SaveChangesAsync();
        }

        public async Task AddFile(Award award, IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                string path = "/img/awards/" + uploadedFile.FileName;

                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }

                award.Image = path;
                await Update(award);
            }
        }

        public void RemoveFile(string path)
        {
            if (!string.IsNullOrEmpty(path) && !GetAll().Any(a => a.Image.Equals(path)))
            {
                var fileInfo = new FileInfo(_appEnvironment.WebRootPath + path);
                fileInfo.Delete();
            }
        }
    }
}
