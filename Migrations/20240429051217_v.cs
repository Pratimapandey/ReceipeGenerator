using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReceipeGenerator.Migrations
{
    public partial class v : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FestivalId",
                table: "Receipes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Festivals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Season = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Festivals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReceipeFestivals",
                columns: table => new
                {
                    ReceipeFestivalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReceipeId = table.Column<int>(type: "int", nullable: false),
                    FestivalId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceipeFestivals", x => x.ReceipeFestivalId);
                    table.ForeignKey(
                        name: "FK_ReceipeFestivals_Festivals_FestivalId",
                        column: x => x.FestivalId,
                        principalTable: "Festivals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReceipeFestivals_Receipes_ReceipeId",
                        column: x => x.ReceipeId,
                        principalTable: "Receipes",
                        principalColumn: "ReceipeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Receipes_FestivalId",
                table: "Receipes",
                column: "FestivalId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceipeFestivals_FestivalId",
                table: "ReceipeFestivals",
                column: "FestivalId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceipeFestivals_ReceipeId",
                table: "ReceipeFestivals",
                column: "ReceipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Receipes_Festivals_FestivalId",
                table: "Receipes",
                column: "FestivalId",
                principalTable: "Festivals",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Receipes_Festivals_FestivalId",
                table: "Receipes");

            migrationBuilder.DropTable(
                name: "ReceipeFestivals");

            migrationBuilder.DropTable(
                name: "Festivals");

            migrationBuilder.DropIndex(
                name: "IX_Receipes_FestivalId",
                table: "Receipes");

            migrationBuilder.DropColumn(
                name: "FestivalId",
                table: "Receipes");
        }
    }
}
