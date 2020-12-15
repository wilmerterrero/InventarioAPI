using Microsoft.EntityFrameworkCore.Migrations;

namespace InventarioAPI.Migrations
{
    public partial class ProductoCodigo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProdCodigo",
                table: "Productos",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProdCodigo",
                table: "Productos");
        }
    }
}
