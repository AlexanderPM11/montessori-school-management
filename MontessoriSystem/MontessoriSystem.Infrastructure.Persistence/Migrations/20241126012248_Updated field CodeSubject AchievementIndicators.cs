using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MontessoriSystem.Infrastructure.Persistence.Migrations
{
    public partial class UpdatedfieldCodeSubjectAchievementIndicators : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdSubject",
                table: "AchievementIndicators");

            migrationBuilder.AddColumn<string>(
                name: "CodeSubject",
                table: "AchievementIndicators",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodeSubject",
                table: "AchievementIndicators");

            migrationBuilder.AddColumn<int>(
                name: "IdSubject",
                table: "AchievementIndicators",
                type: "int",
                nullable: true);
        }
    }
}
