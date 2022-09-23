using DbWriterService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Common.Database.Database
{
    public class ControlcyDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public ControlcyDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sql server with connection string from app settings
            options.UseNpgsql(Configuration.GetConnectionString("ControlcyDatabase"));
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Port> Ports { get; set; }
        public DbSet<Scan> Scans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Port>()
                .HasIndex(x => x.ScanId);
            modelBuilder.Entity<Scan>()
                .HasIndex(x => x.AddressIp);
            /*            modelBuilder.Entity<Address>()
                            .HasMany(s => s.Scans)
                            .WithOne(c => c.Address)
                            .HasPrincipalKey(x => x.Ip);
                        modelBuilder.Entity<Scan>()
                           .HasOne(s => s.Address)
                           .WithMany(c => c.Scans)
                           .HasForeignKey(c => c.AddressIp);
                        modelBuilder.Entity<Port>()
                             .HasOne(y => y.Scan)
                             .WithMany(x => x.Ports)
                             .HasForeignKey(s => s.ScanId);*/

        }
    }
}
