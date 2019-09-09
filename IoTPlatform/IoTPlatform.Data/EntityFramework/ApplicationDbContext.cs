using IoTPlatform.Data.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IoTPlatform.Data.Device;

namespace IoTPlatform.Data.EntityFramework
{
    public class ApplicationDbContext : IdentityDbContext<WebProfileUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Device.Device> Devices { get; set; }
        public DbSet<Measurement> Measurements { get; set; }
        public DbSet<Node> Nodes { get; set; }
        public DbSet<Unit> Units { get; set; }

    }
}