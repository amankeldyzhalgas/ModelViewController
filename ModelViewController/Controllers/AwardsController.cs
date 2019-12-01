using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelViewController.DAL.Entities;
using ModelViewController.Services.Abstract;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ModelViewController.Controllers
{
    public class AwardsController : Controller
    {
        private readonly IRepository<Award> _repository;

        public AwardsController(IRepository<Award> repository)
        {
            _repository = repository;
        }

        // GET: Awards
        public async Task<IActionResult> Index()
        {
            return View(await _repository.GetAllAsync());
        }

        // GET: Awards/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var award = await _repository.Find(id);
            if (award == null)
            {
                return NotFound();
            }

            return View(award);
        }

        // GET: Awards/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Awards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description")] Award award, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                await _repository.Add(award);
                if (image != null)
                {
                    await _repository.AddFile(award, image);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(award);
        }

        // GET: Awards/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var award = await _repository.Find(id);
            if (award == null)
            {
                return NotFound();
            }
            return View(award);
        }

        // POST: Awards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,Description")] Award award, IFormFile image)
        {
            if (id != award.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _repository.Update(award);
                    if (image != null)
                    {
                        await _repository.AddFile(award, image);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AwardExists(award.Id))
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
            return View(award);
        }

        // GET: Awards/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var award = await _repository.Find(id);
            if (award == null)
            {
                return NotFound();
            }

            return View(award);
        }

        // POST: Awards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _repository.Remove(id);
            return RedirectToAction(nameof(Index));
        }

        private bool AwardExists(Guid id)
        {
            return _repository.GetAll().Any(e => e.Id == id);
        }
    }
}
