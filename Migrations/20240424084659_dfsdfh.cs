using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReceipeGenerator.Migrations
{
    public partial class dfsdfh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IngredientId",
                table: "RecipeCreationRequests");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IngredientId",
                table: "RecipeCreationRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
