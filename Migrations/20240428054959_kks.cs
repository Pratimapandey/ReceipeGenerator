using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReceipeGenerator.Migrations
{
    public partial class kks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IngredientWithMeasurement");

            migrationBuilder.DropTable(
                name: "RecipeCreationRequests");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RecipeCreationRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeTitle = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeCreationRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IngredientWithMeasurement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeasurementUnit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
    }
}
