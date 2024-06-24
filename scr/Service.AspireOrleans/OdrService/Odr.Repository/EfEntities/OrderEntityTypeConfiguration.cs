using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Odr.Repository.Entities;

namespace Odr.Repository.EfEntities
{
    internal class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(order => order.Uid).HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.None);
            builder.Property(order => order.CustomerUid).HasColumnName("CustomerUid").IsRequired();
            builder.Property(order => order.StoreUid).HasColumnName("StoreUid").IsRequired();
            builder.Property(order => order.RiderUid).HasColumnName("RiderUid").IsRequired(false);
            builder.Property(order => order.CreateTime).HasColumnName("CreateTime").IsRequired();
            builder.Property(order => order.FinishTime).HasColumnName("FinishTime").IsRequired(false);
            builder.Property(order => order.StartPrice).HasColumnName("StartPrice").IsRequired();
            builder.Property(order => order.DeliveryPrice).HasColumnName("DeliveryPrice").IsRequired();
            builder.Property(order => order.Statu).HasColumnName("Statu").IsRequired();
            builder.Property(order => order.IsDelete).HasColumnName("IsDelete").IsRequired();
        }
    }
}
