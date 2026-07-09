using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MontessoriSystem.Infrastructure.Persistence.Migrations
{
    public partial class RemovingrelashonshipEvaluationsPeriodandAchievementIndicators : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvaluationsPeriod_AchievementIndicators_IdAchievementIndicat~",
                table: "EvaluationsPeriod");

            migrationBuilder.DropIndex(
                name: "IX_EvaluationsPeriod_IdAchievementIndicator",
                table: "EvaluationsPeriod");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_EvaluationsPeriod_IdAchievementIndicator",
                table: "EvaluationsPeriod",
                column: "IdAchievementIndicator");

            migrationBuilder.AddForeignKey(
                name: "FK_EvaluationsPeriod_AchievementIndicators_IdAchievementIndicat~",
                table: "EvaluationsPeriod",
                column: "IdAchievementIndicator",
                principalTable: "AchievementIndicators",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
