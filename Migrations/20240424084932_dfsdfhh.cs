using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReceipeGenerator.Migrations
{
    public partial class dfsdfhh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Receipes_Ingredient",
                table: "Ingredients");

            migrationBuilder.RenameColumn(
                name: "Ingredient",
                table: "Ingredients",
                newName: "ReceipeId");

            migrationBuilder.RenameIndex(
                name: "IX_Ingredients_Ingredient",
                table: "Ingredients",
                newName: "IX_Ingredients_ReceipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Receipes_ReceipeId",
                table: "Ingredients",
                column: "ReceipeId",
                principalTable: "Receipes",
                principalColumn: "ReceipeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Receipes_ReceipeId",
                table: "Ingredients");

            migrationBuilder.RenameColumn(
                name: "ReceipeId",
                table: "Ingredients",
                newName: "Ingredient");

            migrationBuilder.RenameIndex(
                name: "IX_Ingredients_ReceipeId",
                table: "Ingredients",
                newName: "IX_Ingredients_Ingredient");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Receipes_Ingredient",
                table: "Ingredients",
                column: "Ingredient",
                principalTable: "Receipes",
                principalColumn: "ReceipeId");
        }
    }
}
