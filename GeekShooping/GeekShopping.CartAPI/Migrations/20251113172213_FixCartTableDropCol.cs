using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeekShopping.CartAPI.Migrations
{
    public partial class FixCartTableDropCol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CartHeaderId",
                table: "cart_detail");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "cart_detail");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
