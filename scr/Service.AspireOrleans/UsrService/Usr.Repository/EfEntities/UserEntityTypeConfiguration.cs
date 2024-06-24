using Usr.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Usr.Repository.EfEntities
{
    internal class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(user => user.Uid).HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.None);
            builder.Property(user => user.Email).HasMaxLength(128).HasColumnName("Email").IsRequired();
            builder.Property(user => user.Password).HasMaxLength(128).HasColumnName("Password").IsRequired();
            builder.Property(user => user.UserName).HasMaxLength(128).HasColumnName("UserName").IsRequired();
            builder.Property(user => user.Status).HasMaxLength(10).HasColumnName("Status").IsRequired();
            builder.Property(user => user.IsDelete).HasMaxLength(10).HasColumnName("IsDelete").IsRequired();
        }
    }
}
