using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sto.Repository.Migrations
{
    /// <inheritdoc />
    public partial class init_property0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "OwnerId",
                table: "Stores",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Stores");
        }
    }
}
