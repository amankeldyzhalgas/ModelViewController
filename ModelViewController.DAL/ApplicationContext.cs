// <copyright file="ApplicationContext.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ModelViewController.DAL
{
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
        /// Gets or sets .
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Gets or sets .
        /// </summary>
        public DbSet<Award> Awards { get; set; }

        /// <summary>
        /// OnModelCreating method.
        /// </summary>
        /// <param name="modelBuilder">ModelBuilder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
        }
    }
}
