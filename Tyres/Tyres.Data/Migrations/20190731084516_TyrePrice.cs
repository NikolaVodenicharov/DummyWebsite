using Microsoft.EntityFrameworkCore.Migrations;

namespace Tyres.Data.Migrations
{
    public partial class TyrePrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "Tyres");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Tyres",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Tyres");

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "Tyres",
                nullable: true);
        }
    }
}
