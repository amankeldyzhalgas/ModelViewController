using Microsoft.EntityFrameworkCore;
using ModelViewController.DAL.Entities;

namespace ModelViewController.DAL
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Award> Awards { get; set; }
    }
}
