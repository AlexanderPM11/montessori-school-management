using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MontessoriSystem.Infrastructure.Persistence.Migrations
{
    public partial class CommentfieldAgeinGrade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grade_EducationalInstitution_IdEducationalInsti",
                table: "Grade");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "Grade");

            migrationBuilder.AlterColumn<int>(
                name: "IdEducationalInsti",
                table: "Grade",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Grade_EducationalInstitution_IdEducationalInsti",
                table: "Grade",
                column: "IdEducationalInsti",
                principalTable: "EducationalInstitution",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grade_EducationalInstitution_IdEducationalInsti",
                table: "Grade");

            migrationBuilder.AlterColumn<int>(
                name: "IdEducationalInsti",
                table: "Grade",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Grade",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Grade_EducationalInstitution_IdEducationalInsti",
                table: "Grade",
                column: "IdEducationalInsti",
                principalTable: "EducationalInstitution",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
