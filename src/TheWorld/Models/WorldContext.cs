using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TheWorld.Models
{
    public class WorldContext : IdentityDbContext
    {
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Stop> Stops { get; set; }
        public DbSet<WorldUser> Users { get; set; }

        public WorldContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connection = Startup.Configuration["Data:WorldContextConnection"];

            optionsBuilder.UseSqlServer(connection);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
