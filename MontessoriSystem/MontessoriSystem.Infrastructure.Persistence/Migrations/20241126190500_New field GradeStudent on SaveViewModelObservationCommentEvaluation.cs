using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MontessoriSystem.Infrastructure.Persistence.Migrations
{
    public partial class NewfieldGradeStudentonSaveViewModelObservationCommentEvaluation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Comment2",
                table: "ObservationCommentEvaluation",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "GradeStudent",
                table: "ObservationCommentEvaluation",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GradeStudent",
                table: "ObservationCommentEvaluation");

            migrationBuilder.UpdateData(
                table: "ObservationCommentEvaluation",
                keyColumn: "Comment2",
                keyValue: null,
                column: "Comment2",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Comment2",
                table: "ObservationCommentEvaluation",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
