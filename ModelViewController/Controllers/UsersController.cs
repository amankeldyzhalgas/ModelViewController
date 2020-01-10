// <copyright file="UsersController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ModelViewController.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;
    using ModelViewController.DAL.Entities;
    using ModelViewController.Models;
    using ModelViewController.Models.Users;
    using ModelViewController.Services;
    using ModelViewController.Services.Abstract;
    using SmartBreadcrumbs.Attributes;

    /// <summary>
    /// Users Controller.
    /// </summary>
    [Authorize]
    [Breadcrumb("Users")]
    public class UsersController : Controller
    {
        private readonly IUserRepository<User> _userRepository;
        private readonly IAwardRepository<Award> _awardRepository;
        private readonly IRoleRepository<Role> _roleRepository;
        private readonly IMemoryCache _cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="userRepository">User repository.</param>
        /// <param name="awardRepository">Award repository.</param>
        /// <param name="roleRepository">Role repository.</param>
        /// <param name="cache">cache.</param>
        public UsersController(
            IUserRepository<User> userRepository,
            IAwardRepository<Award> awardRepository,
            IRoleRepository<Role> roleRepository,
            IMemoryCache cache)
        {
            this._userRepository = userRepository;
            this._awardRepository = awardRepository;
            this._roleRepository = roleRepository;
            this._cache = cache;
        }

        // GET: Users

        /// <summary>
        /// Index method.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [AllowAnonymous]
        [Route("/users")]
        public async Task<IActionResult> Index()
        {
            /*this._cache.TryGetValue(this.User.Identity.Name, out CacheModel changes);
            if (changes != null)
            {
                this.ViewBag.CacheDatas = changes;
                this.ViewBag.CacheIsEmpty = false;
            }
            else
            {
                this.ViewBag.CacheIsEmpty = true;
            }*/

            return this.View(await this._userRepository.GetAllAsync());
        }

        /// <summary>
        /// Index method with param.
        /// </summary>
        /// <param name="name">name.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Route("/users/{name}")]
        public async Task<IActionResult> Index(string name)
        {
            /*this._cache.TryGetValue(this.User.Identity.Name, out CacheModel changes);
            if (changes != null)
            {
                this.ViewBag.CacheDatas = changes;
                this.ViewBag.CacheIsEmpty = false;
            }
            else
            {
                this.ViewBag.CacheIsEmpty = true;
            }*/

            return this.View(await this._userRepository.Filter(name));
        }

        /// <summary>
        /// .
        /// </summary>
        /// <param name="id">User Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        // GET: Users/Details/5
        [Breadcrumb("Details")]
        [Route("user/{id}")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            if (Guid.TryParse(id, out Guid guid))
            {
                var user = await this._userRepository.Find(guid);
                return this.View(user);
            }

            var users = await this._userRepository.Filter(id);
            if (users != null)
            {
                var user = users.First();
                return this.View(user);
            }

            return this.NotFound();
        }

        // GET: Users/Create

        /// <summary>
        /// .
        /// </summary>
        /// <returns>Create View.</returns>
        [Breadcrumb("Create")]
        [Authorize(Roles = "admin, candidate")]
        [Route("/create-user")]
        public IActionResult Create()
        {
            this.ViewData["Awards"] = this._awardRepository.GetAll();
            return this.View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        /// <summary>
        /// .
        /// </summary>
        /// <param name="model">UserModel.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Breadcrumb("Create")]
        [Authorize(Roles = "admin, candidate")]
        [Route("/create-user")]
        public async Task<IActionResult> Create(UserModel model)
        {
            if (this.ModelState.IsValid)
            {
                if (model.Birthdate > DateTime.Now || DateTime.Now.Year - 150 > model.Birthdate.Year)
                {
                    this.ModelState.AddModelError(string.Empty, "The age of the user can not be negative, can not be more than 150 years.");

                    this.ViewData["Awards"] = this._awardRepository.GetAll();
                    return this.View(model);
                }

                if (this.User.IsInRole("candidate"))
                {
                    // TODO: Add Cache
                    this.AddCache(model, Models.Action.Add);
                    return this.RedirectToAction(nameof(this.Index));
                }

                var user = new User { Name = model.Name, Birthdate = model.Birthdate };
                user = await this._userRepository.Add(user);

                if (model.Awards.UserAwards != null)
                {
                    var awards = model.Awards.UserAwards.Where(ua => ua.Selected).Select(ua => ua.AwardId).ToList();
                    await this._userRepository.UpdateUserAwards(user, awards);
                }

                if (model.Photo != null)
                {
                    await this._userRepository.AddFile(user, model.Photo);
                }

                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(model);
        }

        /// <summary>
        /// .
        /// </summary>
        /// <param name="id">id.</param>
        /// <returns>View.</returns>
        [Breadcrumb("AddAwards")]
        public async Task<IActionResult> AddAwards(Guid? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var user = await this._userRepository.Find(id);
            var userAwards = await this._userRepository.GetUserAwards(id);
            var awards = await this._awardRepository.GetAllAsync();

            List<UserAwardModel> userAwardModels = new List<UserAwardModel>();
            foreach (var award in awards)
            {
                UserAwardModel userAwardModel = new UserAwardModel
                {
                    AwardId = award.Id,
                    Title = award.Title,
                    Image = award.Image,
                    Selected = userAwards.Contains(award),
                };
                userAwardModels.Add(userAwardModel);
            }

            ChangeUserAwardModel changeUserAwardModel = new ChangeUserAwardModel
            {
                UserId = user.Id,
                UserAwards = userAwardModels,
            };

            if (user == null)
            {
                return this.NotFound();
            }

            return this.View(changeUserAwardModel);
        }

        /// <summary>
        /// .
        /// </summary>
        /// <param name="id">id.</param>
        /// <param name="model">model.</param>
        /// <returns>View.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Breadcrumb("AddAwards")]
        public async Task<IActionResult> AddAwards(Guid id, ChangeUserAwardModel model)
        {
            if (id != model.UserId)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    var user = await this._userRepository.Find(id);
                    if (model.UserAwards != null)
                    {
                        var awards = model.UserAwards.Where(ur => ur.Selected).Select(ur => ur.AwardId).ToList();
                        if (awards != null)
                        {
                            await this._userRepository.UpdateUserAwards(user, awards);
                        }
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.UserExists(model.UserId))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(model);
        }

        /*
            award-user/10_25 - Награждение пользователя с идентификатором 10
            наградой с идентификатором 25
        */

        /// <summary>
        /// AddAward.
        /// </summary>
        /// <param name="id">id.</param>
        /// <param name="awardId">awardId.</param>
        /// <returns>awards.</returns>
        [Route("/award-user/{id:Guid}/{awardId:Guid}")]
        public async Task<IActionResult> AddAward(Guid id, Guid awardId)
        {
            try
            {
                var user = await this._userRepository.Find(id);

                var award = await this._awardRepository.Find(awardId);
                if (award != null)
                {
                    var awards = new List<Award>();
                    awards.Add(award);
                }

                await this._userRepository.AddUserAwards(user, awardId);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.UserExists(id))
                {
                    return this.NotFound();
                }
                else
                {
                    throw;
                }
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        /// <summary>
        /// .
        /// </summary>
        /// <param name="id">User Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        // GET: Users/Edit/5
        [Breadcrumb("Edit")]
        [Authorize(Roles = "admin, candidate")]
        [Route("/user/{id}/edit")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var user = await this._userRepository.Find(id);
            var userAwards = await this._userRepository.GetUserAwards(id);
            var awards = await this._awardRepository.GetAllAsync();

            List<UserAwardModel> userAwardModels = new List<UserAwardModel>();
            foreach (var award in awards)
            {
                UserAwardModel userAwardModel = new UserAwardModel
                {
                    AwardId = award.Id,
                    Title = award.Title,
                    Image = award.Image,
                    Selected = userAwards.Contains(award),
                };
                userAwardModels.Add(userAwardModel);
            }

            ChangeUserAwardModel changeUserAwardModel = new ChangeUserAwardModel
            {
                UserId = user.Id,
                UserAwards = userAwardModels,
            };

            var model = new UserModel { Id = user.Id, Name = user.Name, Birthdate = user.Birthdate, PhotoSrc = user.Photo, Awards = changeUserAwardModel };
            if (user == null)
            {
                return this.NotFound();
            }

            return this.View(model);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        /// <summary>
        /// .
        /// </summary>
        /// <param name="id">User Id.</param>
        /// <param name="model">Model.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Breadcrumb("Edit")]
        [Authorize(Roles = "admin, candidate")]
        [Route("/user/{id}/edit")]
        public async Task<IActionResult> Edit(Guid id, UserModel model)
        {
            if (id != model.Id)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    var user = await this._userRepository.Find(id);
                    user.Name = model.Name;
                    user.Birthdate = model.Birthdate;

                    var awards = model.Awards.UserAwards.Where(ur => ur.Selected).Select(ur => ur.AwardId).ToList();
                    if (awards != null)
                    {
                        await this._userRepository.UpdateUserAwards(user, awards);
                    }

                    if (model.Photo != null)
                    {
                        await this._userRepository.AddFile(user, model.Photo);
                    }

                    await this._userRepository.Update(user);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.UserExists(model.Id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return this.RedirectToAction(nameof(this.Index));
            }

            this.ViewData["Awards"] = this._awardRepository.GetAll();
            return this.View(model);
        }

        /// <summary>
        /// ChangeRole method.
        /// </summary>
        /// <param name="id">id.</param>
        /// <returns>View.</returns>
        [Breadcrumb("ChangeRole")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangeRole(Guid? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var user = await this._userRepository.Find(id);
            var userRoles = await this._userRepository.GetUserRoles(id);
            var roles = await this._roleRepository.GetAllAsync();

            List<UserRoleModel> roleModels = new List<UserRoleModel>();
            foreach (var role in roles)
            {
                UserRoleModel roleModel = new UserRoleModel
                {
                    RoleId = role.Id,
                    Name = role.DisplayName,
                    Selected = userRoles.Contains(role),
                };
                roleModels.Add(roleModel);
            }

            ChangeUserRolesModel changeUserRoles = new ChangeUserRolesModel
            {
                UserId = user.Id,
                UserName = user.Name,
                UserRoles = roleModels,
            };

            if (user == null)
            {
                return this.NotFound();
            }

            return this.View(changeUserRoles);
        }

        /// <summary>
        /// .
        /// </summary>
        /// <param name="userId">UserId.</param>
        /// <param name="model">model.</param>
        /// <returns>View.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Breadcrumb("ChangeRole")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangeRole(Guid userId, [Bind("UserId,UserName,UserRoles")] ChangeUserRolesModel model)
        {
            if (userId != model.UserId)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    var user = await this._userRepository.Find(userId);
                    var roles = model.UserRoles.Where(ur => ur.Selected).Select(ur => ur.RoleId).ToList();

                    await this._userRepository.UpdateUserRoles(user, roles);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.UserExists(model.UserId))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        // GET: Users/Delete/5

        /// <summary>
        /// .
        /// </summary>
        /// <param name="id">User Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [Breadcrumb("Delete")]
        [Authorize(Roles = "admin, candidate")]
        [Route("/user/{id}/delete")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var user = await this._userRepository.Find(id);
            if (user == null)
            {
                return this.NotFound();
            }

            return this.View(user);
        }

        // POST: Users/Delete/5

        /// <summary>
        /// .
        /// </summary>
        /// <param name="id">User Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        [Authorize(Roles = "admin, candidate")]
        [Route("/user/{id}/delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await this._userRepository.Remove(id);
            return this.RedirectToAction(nameof(this.Index));
        }

        /// <summary>
        /// .
        /// </summary>
        /// <returns>Physical File.</returns>
        public IActionResult Download()
        {
            var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "wwwroot", "users.txt");
            using (var stream = new FileStream(path, FileMode.OpenOrCreate))
            {
                foreach (var item in this._userRepository.GetAll())
                {
                    var str = $"Name: {item.Name} Birthdate: {item.Birthdate} Photo: {item.Photo}{Environment.NewLine}Awards{Environment.NewLine}";
                    if (item.UserAwards != null)
                    {
                        foreach (var award in item.UserAwards)
                        {
                            str += $"Title: {award.Award.Title} ";
                        }

                        str += $"{Environment.NewLine}";
                    }
                    else
                    {
                        str += $"Empty.{Environment.NewLine}";
                    }

                    byte[] array = System.Text.Encoding.Default.GetBytes(str);
                    stream.Write(array, 0, array.Length);
                }
            }

            return this.PhysicalFile(path, "text/plain", "users.txt");
        }

        private bool UserExists(Guid id)
        {
            return this._userRepository.GetAll().Any(e => e.Id == id);
        }

        // TODO: action{add,update,remove}
        private void AddCache(UserModel model, Models.Action action)
        {
            if (!this._cache.TryGetValue(this.User.Identity.Name, out CacheModel cacheValues))
            {
                cacheValues = new CacheModel()
                {
                    AddedUsers = new List<UserModel>(),
                    UpdatedUsers = new List<UserModel>(),
                    DeletedUsers = new List<UserModel>(),
                    AddedAwards = new List<AwardModel>(),
                    UpdatedAwards = new List<AwardModel>(),
                    DeletedAwards = new List<AwardModel>(),
                };
            }

            switch (action)
            {
                case Models.Action.Add:
                    cacheValues.AddedUsers.Add(model);
                    break;
                case Models.Action.Update:
                    cacheValues.UpdatedUsers.Add(model);
                    break;
                case Models.Action.Remove:
                    cacheValues.DeletedUsers.Add(model);
                    break;
                default:
                    break;
            }

            this._cache.Set(
                this.User.Identity.Name,
                cacheValues,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(300)));
        }
    }
}
