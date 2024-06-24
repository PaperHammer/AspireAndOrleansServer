using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sto.Repository.Entities;

namespace Sto.Repository.EfEntities
{
    internal class StoreEntityTypeConfiguration : IEntityTypeConfiguration<Store>
    {
        public void Configure(EntityTypeBuilder<Store> builder)
        {
            builder.ToTable("Stores");

            builder.HasKey(store => store.Uid).HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.None);
            builder.Property(store => store.StoreImage).HasColumnName("StoreImage").IsRequired(false);
            builder.Property(store => store.StoreName).HasMaxLength(50).HasColumnName("StoreName").IsRequired();            
            builder.Property(store => store.Position).HasMaxLength(256).HasColumnName("Position").IsRequired();            
            builder.Property(store => store.Score).HasColumnName("Score").IsRequired();            
            builder.Property(store => store.StoreDesc).HasMaxLength(256).HasColumnName("StoreDesc").IsRequired(false);
            builder.Property(store => store.OpeningTime).HasColumnName("OpeningTime").IsRequired();
            builder.Property(store => store.ClosingTime).HasColumnName("ClosingTime").IsRequired();
            builder.Property(store => store.StartPrice).HasColumnName("StartPrice").IsRequired();
            builder.Property(store => store.DeliveryPrice).HasColumnName("DeliveryPrice").IsRequired();
            builder.Property(store => store.IsDelete).HasColumnName("IsDelete").IsRequired();
        }
    }
}
