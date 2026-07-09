using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MontessoriSystem.Infrastructure.Persistence.Migrations
{
    public partial class ChangedfieldnameSubjecttoIdSubjectonEvaluationsPeriodWithCalification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GradeStudent",
                table: "EvaluationsPeriodWithCalification");

            migrationBuilder.DropColumn(
                name: "Subject",
                table: "EvaluationsPeriodWithCalification");

            migrationBuilder.AddColumn<int>(
                name: "IdSubject",
                table: "EvaluationsPeriodWithCalification",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdSubject",
                table: "EvaluationsPeriodWithCalification");

            migrationBuilder.AddColumn<string>(
                name: "GradeStudent",
                table: "EvaluationsPeriodWithCalification",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "EvaluationsPeriodWithCalification",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
