using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseProject.Migrations
{
    public partial class PriceBugFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "decimal(18,2)",
                table: "Products",
                newName: "Price");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Products",
                newName: "decimal(18,2)");
        }
    }
}
