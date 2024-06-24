using Microsoft.EntityFrameworkCore;
using Sto.Repository.EfEntities;
using Sto.Repository.Entities;

namespace Sto.Repository.Core
{
    public class StoreDbContext : DbContext
    {
        public DbSet<Store> Stores { get; set; }

        public StoreDbContext() { }

        public StoreDbContext(DbContextOptions<StoreDbContext> options)
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
            modelBuilder.ApplyConfiguration(new StoreEntityTypeConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
