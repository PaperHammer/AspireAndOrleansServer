﻿// <auto-generated />
using Prdt.Repository.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Prdt.Repository.Migrations
{
    [DbContext(typeof(ProductDbContext))]
    [Migration("20240611142911_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Prdt.Repository.Entities.Product", b =>
                {
                    b.Property<long>("Uid")
                        .HasColumnType("bigint");

                    b.Property<long>("CategoryUid")
                        .HasColumnType("bigint")
                        .HasColumnName("CategoryUid");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit")
                        .HasColumnName("IsDelete");

                    b.Property<string>("ProductDesc")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)")
                        .HasColumnName("ProductDesc");

                    b.Property<byte[]>("ProductImage")
                        .IsRequired()
                        .HasColumnType("varbinary(max)")
                        .HasColumnName("ProductImage");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("ProductName");

                    b.Property<double>("ProductPrice")
                        .HasColumnType("float")
                        .HasColumnName("ProductPrice");

                    b.Property<long>("StoreUid")
                        .HasMaxLength(128)
                        .HasColumnType("bigint")
                        .HasColumnName("StoreUid");

                    b.HasKey("Uid")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.None);

                    b.ToTable("Products", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
