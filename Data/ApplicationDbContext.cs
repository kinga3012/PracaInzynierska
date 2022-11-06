using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PracaInzynierska.Models;

namespace PracaInzynierska.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Monument> Monuments { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Category> Categories { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext(new DbContextOptions<ApplicationDbContext>());
        }
    }
}