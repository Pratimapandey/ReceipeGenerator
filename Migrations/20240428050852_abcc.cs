using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReceipeGenerator.Migrations
{
    public partial class abcc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_RecipeCreationRequests_RecipeCreationRequestId",
                table: "Ingredients");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_RecipeCreationRequestId",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "RecipeCreationRequestId",
                table: "Ingredients");

            migrationBuilder.CreateTable(
                name: "IngredientWithMeasurement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MeasurementUnit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecipeCreationRequestId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngredientWithMeasurement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IngredientWithMeasurement_RecipeCreationRequests_RecipeCreationRequestId",
                        column: x => x.RecipeCreationRequestId,
                        principalTable: "RecipeCreationRequests",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_IngredientWithMeasurement_RecipeCreationRequestId",
                table: "IngredientWithMeasurement",
                column: "RecipeCreationRequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IngredientWithMeasurement");

            migrationBuilder.AddColumn<int>(
                name: "RecipeCreationRequestId",
                table: "Ingredients",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_RecipeCreationRequestId",
                table: "Ingredients",
                column: "RecipeCreationRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_RecipeCreationRequests_RecipeCreationRequestId",
                table: "Ingredients",
                column: "RecipeCreationRequestId",
                principalTable: "RecipeCreationRequests",
                principalColumn: "Id");
        }
    }
}
