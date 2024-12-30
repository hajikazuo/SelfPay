using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SelfPay.Common.Models;
using SelfPay.Common.Models.Users;

namespace SelfPay.Common.Data
{
    public class SelfPayContext : IdentityDbContext<User, Role, Guid>
    {
        public SelfPayContext(DbContextOptions<SelfPayContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ClientInfo> ClientInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Service>()
                .Property(s => s.Price15)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Service>()
                .Property(s => s.Price20)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Service>()
                .Property(s => s.Price30)
                .HasPrecision(10, 2);
        }

    }
}
