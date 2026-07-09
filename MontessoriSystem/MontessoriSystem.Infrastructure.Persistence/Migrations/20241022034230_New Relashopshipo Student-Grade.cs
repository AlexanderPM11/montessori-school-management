using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MontessoriSystem.Infrastructure.Persistence.Migrations
{
    public partial class NewRelashopshipoStudentGrade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdGrade",
                table: "Student",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Student_IdGrade",
                table: "Student",
                column: "IdGrade");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Grade_IdGrade",
                table: "Student",
                column: "IdGrade",
                principalTable: "Grade",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_Grade_IdGrade",
                table: "Student");

            migrationBuilder.DropIndex(
                name: "IX_Student_IdGrade",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "IdGrade",
                table: "Student");
        }
    }
}
