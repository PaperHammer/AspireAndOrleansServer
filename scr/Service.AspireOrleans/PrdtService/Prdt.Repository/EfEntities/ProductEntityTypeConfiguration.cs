using Prdt.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Prdt.Repository.EfEntities
{
    internal class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(product => product.Uid).HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.None);
            builder.Property(product => product.CategoryUid).HasColumnName("CategoryUid").IsRequired();
            builder.Property(product => product.StoreUid).HasMaxLength(128).HasColumnName("StoreUid").IsRequired();
            builder.Property(product => product.ProductImage).HasColumnName("ProductImage").IsRequired(false);
            builder.Property(product => product.ProductName).HasMaxLength(50).HasColumnName("ProductName").IsRequired();
            builder.Property(product => product.ProductDesc).HasMaxLength(256).HasColumnName("ProductDesc").IsRequired(false);
            builder.Property(product => product.ProductPrice).HasColumnName("ProductPrice").IsRequired();
            builder.Property(product => product.IsDelete).HasColumnName("IsDelete").IsRequired();
        }
    }
}
