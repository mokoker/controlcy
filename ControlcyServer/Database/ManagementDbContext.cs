using ControlcyServer.Models;
using Microsoft.EntityFrameworkCore;

namespace ControlcyServer.Database
{
    public class ManagementDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public ManagementDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sql server with connection string from app settings
            options.UseNpgsql(Configuration.GetConnectionString("ControlcyDatabase"));
        }

        public DbSet<Segment> Segments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
       


        }
    }
}
