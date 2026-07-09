using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MontessoriSystem.Infrastructure.Persistence.Migrations
{
    public partial class NewfieldonSaveAchievementIndicatorsViewModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "AchievementIndicators",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subject",
                table: "AchievementIndicators");
        }
    }
}
