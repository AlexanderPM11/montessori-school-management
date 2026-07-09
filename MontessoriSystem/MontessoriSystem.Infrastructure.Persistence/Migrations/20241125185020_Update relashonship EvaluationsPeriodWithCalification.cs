using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MontessoriSystem.Infrastructure.Persistence.Migrations
{
    public partial class UpdaterelashonshipEvaluationsPeriodWithCalification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvaluationsPeriodWithCalification_Student_StudentId",
                table: "EvaluationsPeriodWithCalification");

            migrationBuilder.DropIndex(
                name: "IX_EvaluationsPeriodWithCalification_StudentId",
                table: "EvaluationsPeriodWithCalification");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "EvaluationsPeriodWithCalification");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationsPeriodWithCalification_IdStudent",
                table: "EvaluationsPeriodWithCalification",
                column: "IdStudent");

            migrationBuilder.AddForeignKey(
                name: "FK_EvaluationsPeriodWithCalification_Student_IdStudent",
                table: "EvaluationsPeriodWithCalification",
                column: "IdStudent",
                principalTable: "Student",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvaluationsPeriodWithCalification_Student_IdStudent",
                table: "EvaluationsPeriodWithCalification");

            migrationBuilder.DropIndex(
                name: "IX_EvaluationsPeriodWithCalification_IdStudent",
                table: "EvaluationsPeriodWithCalification");

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "EvaluationsPeriodWithCalification",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationsPeriodWithCalification_StudentId",
                table: "EvaluationsPeriodWithCalification",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_EvaluationsPeriodWithCalification_Student_StudentId",
                table: "EvaluationsPeriodWithCalification",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
