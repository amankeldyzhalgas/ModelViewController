// <copyright file="ApplicationContext.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ModelViewController.DAL
{
    using System;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using ModelViewController.DAL.Entities;

    /// <summary>
    /// Application Context.
    /// </summary>
    public class ApplicationContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationContext"/> class.
        /// </summary>
        /// <param name="options">DbContextOptions.</param>
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            this.Database.EnsureCreated();
        }

        /// <summary>
        /// Gets or sets Users.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Gets or sets Awards.
        /// </summary>
        public DbSet<Award> Awards { get; set; }

        /// <summary>
        /// Gets or sets Roles.
        /// </summary>
        public DbSet<Role> Roles { get; set; }

        /// <summary>
        /// OnModelCreating method.
        /// </summary>
        /// <param name="modelBuilder">ModelBuilder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region UserAwards
            modelBuilder.Entity<UserAward>()
                .HasKey(t => new { t.UserId, t.AwardId });

            modelBuilder.Entity<UserAward>()
                .HasOne(ua => ua.User)
                .WithMany(u => u.UserAwards)
                .HasForeignKey(ua => ua.UserId);

            modelBuilder.Entity<UserAward>()
                .HasOne(ua => ua.Award)
                .WithMany(a => a.UserAwards)
                .HasForeignKey(ua => ua.AwardId);
            #endregion
            #region UserRoles
            modelBuilder.Entity<UserRole>()
                .HasKey(t => new { t.UserId, t.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);
            #endregion

            modelBuilder.Entity<User>(user => {
                user.HasIndex(u => u.UserName).IsUnique();
            });

            var roleId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var roles = new[]
            {
                new Role { Id = roleId, Name = "admin", DisplayName = "Администратор" },
                new Role { Id = Guid.NewGuid(), Name = "user", DisplayName = "Пользователь" },
            };

            var users = new[]
            {
                new User { Id = userId, UserName = "admin", Password = "admin", Name = "admin", Birthdate = DateTime.Parse("12.12.2012") },
            };

            var userRoles = new[]
            {
                new UserRole { UserId = userId, RoleId = roleId },
            };
            modelBuilder.Entity<User>().HasData(users[0]);
            modelBuilder.Entity<Role>().HasData(roles);
            modelBuilder.Entity<UserRole>().HasData(userRoles[0]);
        }
    }
}
