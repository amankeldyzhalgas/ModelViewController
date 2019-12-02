using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelViewController.DAL.Entities;
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
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Birthdate")] User user, IFormFile photo)
        {
            if (ModelState.IsValid)
            {
                await _repository.Add(user);
                if (photo != null)
                {
                    await _repository.AddFile(user, photo);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
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

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Birthdate,Photo")] User user, IFormFile photo, int[] selectedAwards)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _repository.Update(user);
                    if (photo != null)
                    {
                        await _repository.AddFile(user, photo);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
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
