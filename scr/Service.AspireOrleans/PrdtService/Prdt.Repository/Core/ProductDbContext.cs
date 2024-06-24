using Prdt.Repository.EfEntities;
using Prdt.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace Prdt.Repository.Core
{
    public class ProductDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public ProductDbContext() { }

        public ProductDbContext(DbContextOptions<ProductDbContext> options)
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
            modelBuilder.ApplyConfiguration(new ProductEntityTypeConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
