using Microsoft.EntityFrameworkCore;
using Odr.Repository.EfEntities;
using Odr.Repository.Entities;

namespace Odr.Repository.Core
{
    public class OrderDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }

        public OrderDbContext() { }

        public OrderDbContext(DbContextOptions<OrderDbContext> options)
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("server=127.0.0.1;database=AspireOrleansDb;uid=sa;pwd=16504;Encrypt=true;Trusted_Connection=true;TrustServerCertificate=true;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
