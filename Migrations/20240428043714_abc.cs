using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReceipeGenerator.Migrations
{
    public partial class abc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CookingTime",
                table: "Receipes");

            migrationBuilder.DropColumn(
                name: "DifficultyLevel",
                table: "Receipes");

            migrationBuilder.DropColumn(
                name: "Instructions",
                table: "Receipes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CookingTime",
                table: "Receipes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DifficultyLevel",
                table: "Receipes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Instructions",
                table: "Receipes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
