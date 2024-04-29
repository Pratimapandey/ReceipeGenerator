using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReceipeGenerator.Migrations
{
    public partial class initialll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IngredientRequests");

            migrationBuilder.CreateTable(
                name: "RecipeCreationRequest",
                columns: table => new
                {
                    RecipeTitle = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeCreationRequest");

            migrationBuilder.CreateTable(
                name: "IngredientRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });
        }
    }
}
