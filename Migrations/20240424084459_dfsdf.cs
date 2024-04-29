using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReceipeGenerator.Migrations
{
    public partial class dfsdf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Receipes_ReceipeId",
                table: "Ingredients");

            migrationBuilder.RenameTable(
                name: "RecipeCreationRequest",
                newName: "RecipeCreationRequests");

            migrationBuilder.RenameColumn(
                name: "ReceipeId",
                table: "Ingredients",
                newName: "RecipeCreationRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_Ingredients_ReceipeId",
                table: "Ingredients",
                newName: "IX_Ingredients_RecipeCreationRequestId");

            migrationBuilder.AddColumn<int>(
                name: "Ingredient",
                table: "Ingredients",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "RecipeCreationRequests",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "IngredientId",
                table: "RecipeCreationRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeCreationRequests",
                table: "RecipeCreationRequests",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_Ingredient",
                table: "Ingredients",
                column: "Ingredient");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Receipes_Ingredient",
                table: "Ingredients",
                column: "Ingredient",
                principalTable: "Receipes",
                principalColumn: "ReceipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_RecipeCreationRequests_RecipeCreationRequestId",
                table: "Ingredients",
                column: "RecipeCreationRequestId",
                principalTable: "RecipeCreationRequests",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Receipes_Ingredient",
                table: "Ingredients");

            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_RecipeCreationRequests_RecipeCreationRequestId",
                table: "Ingredients");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_Ingredient",
                table: "Ingredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeCreationRequests",
                table: "RecipeCreationRequests");

            migrationBuilder.DropColumn(
                name: "Ingredient",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "RecipeCreationRequests");

            migrationBuilder.DropColumn(
                name: "IngredientId",
                table: "RecipeCreationRequests");

            migrationBuilder.RenameTable(
                name: "RecipeCreationRequests",
                newName: "RecipeCreationRequest");

            migrationBuilder.RenameColumn(
                name: "RecipeCreationRequestId",
                table: "Ingredients",
                newName: "ReceipeId");

            migrationBuilder.RenameIndex(
                name: "IX_Ingredients_RecipeCreationRequestId",
                table: "Ingredients",
                newName: "IX_Ingredients_ReceipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Receipes_ReceipeId",
                table: "Ingredients",
                column: "ReceipeId",
                principalTable: "Receipes",
                principalColumn: "ReceipeId");
        }
    }
}
