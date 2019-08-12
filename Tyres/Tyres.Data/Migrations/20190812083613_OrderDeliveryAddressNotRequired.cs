using Microsoft.EntityFrameworkCore.Migrations;

namespace Tyres.Data.Migrations
{
    public partial class OrderDeliveryAddressNotRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DeliveryAddress",
                table: "Orders",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 200);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DeliveryAddress",
                table: "Orders",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 200,
                oldNullable: true);
        }
    }
}
