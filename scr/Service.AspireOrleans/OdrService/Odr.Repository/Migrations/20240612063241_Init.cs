using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Odr.Repository.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Uid = table.Column<long>(type: "bigint", nullable: false),
                    CustomerUid = table.Column<long>(type: "bigint", nullable: false),
                    StoreUid = table.Column<long>(type: "bigint", nullable: false),
                    RiderUid = table.Column<long>(type: "bigint", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinishTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartPrice = table.Column<double>(type: "float", nullable: false),
                    DeliveryPrice = table.Column<double>(type: "float", nullable: false),
                    Statu = table.Column<int>(type: "int", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Uid);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
