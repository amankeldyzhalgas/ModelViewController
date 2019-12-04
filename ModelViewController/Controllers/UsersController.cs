using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ModelViewController.DAL.Entities;
using ModelViewController.Models;
using ModelViewController.Services.Abstract;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ModelViewController.Controllers
{
    public class UsersController : Controller
    {
        private readonly IRepository<User> _repository;
        private readonly IRepository<Award> _awardRepository;

        public UsersController(IRepository<User> repository, IRepository<Award> awardRepository)
        {
            _repository = repository;
            _awardRepository = awardRepository;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _repository.GetAllAsync());
        }

        //public IActionResult Users()
        //{
        //    List<UserViewModel> users = _repository.GetAll()
        //        .Select(u => new UserViewModel
        //        {
        //            Id = u.Id,
        //            Name = u.Name,
        //            Birthdate = u.Birthdate,
        //            Age = DateTime.Now.Year - u.Birthdate.Year
        //        }).ToList();
        //    return View();
        //}

        // GET: Users/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _repository.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["Awards"] = _awardRepository.GetAll();
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Birthdate > DateTime.Now || DateTime.Now.Year - 150 > model.Birthdate.Year)
                {
                    ModelState.AddModelError("", "The age of the user can not be negative, can not be more than 150 years.");
                    return View(model);
                }
                var user = new User { Name = model.Name, Birthdate = model.Birthdate };
                await _repository.Add(user);
                if (model.Photo != null)
                {
                    await _repository.AddFile(user, model.Photo);
                }
                if(model.Awards != null)
                {
                    //add awards
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["Awards"] = _awardRepository.GetAll();
            return View(model);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _repository.Find(id);
            var model = new UserModel { Id = user.Id, Name = user.Name, Birthdate = user.Birthdate };
            if (user == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, UserModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _repository.Find(id);
                    user.Name = model.Name;
                    user.Birthdate = model.Birthdate;
                    await _repository.Update(user);
                    if (model.Photo != null)
                    {
                        await _repository.AddFile(user, model.Photo);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _repository.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _repository.Remove(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Download()
        {
            var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "wwwroot", "users.txt");
            using (var stream = new FileStream(path, FileMode.OpenOrCreate))
            {
                foreach (var item in _repository.GetAll())
                {
                    var str = $"Name: {item.Name} Birthdate: {item.Birthdate} Photo: {item.Photo}\n";
                    byte[] array = System.Text.Encoding.Default.GetBytes(str);
                    stream.Write(array, 0, array.Length);
                }
            }

            return PhysicalFile(path, "text/plain", "users.txt");
        }

        private bool UserExists(Guid id)
        {
            return _repository.GetAll().Any(e => e.Id == id);
        }
    }
}
