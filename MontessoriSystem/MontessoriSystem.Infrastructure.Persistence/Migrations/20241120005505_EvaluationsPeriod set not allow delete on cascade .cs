using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MontessoriSystem.Infrastructure.Persistence.Migrations
{
    public partial class EvaluationsPeriodsetnotallowdeleteoncascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvaluationsPeriod_AchievementIndicators_IdAchievementIndicat~",
                table: "EvaluationsPeriod");

            migrationBuilder.AlterColumn<int>(
                name: "IdAchievementIndicator",
                table: "EvaluationsPeriod",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_EvaluationsPeriod_AchievementIndicators_IdAchievementIndicat~",
                table: "EvaluationsPeriod",
                column: "IdAchievementIndicator",
                principalTable: "AchievementIndicators",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvaluationsPeriod_AchievementIndicators_IdAchievementIndicat~",
                table: "EvaluationsPeriod");

            migrationBuilder.AlterColumn<int>(
                name: "IdAchievementIndicator",
                table: "EvaluationsPeriod",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EvaluationsPeriod_AchievementIndicators_IdAchievementIndicat~",
                table: "EvaluationsPeriod",
                column: "IdAchievementIndicator",
                principalTable: "AchievementIndicators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
