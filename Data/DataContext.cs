using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Data
{
    public class DataContext : IdentityDbContext<IdentityUser>
    {
        public DataContext(DbContextOptions<DataContext> options): base(options)
        {

        }
        public DbSet<City> Cities { get; set; }

        public DbSet<Property> Properties { get; set; }

        public DbSet<User> Users { get; set; }
    }

}
