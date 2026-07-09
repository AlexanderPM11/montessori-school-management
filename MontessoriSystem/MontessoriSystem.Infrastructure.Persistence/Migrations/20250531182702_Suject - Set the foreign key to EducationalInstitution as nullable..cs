using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MontessoriSystem.Infrastructure.Persistence.Migrations
{
    public partial class SujectSettheforeignkeytoEducationalInstitutionasnullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Suject_EducationalInstitution_IdEducationalInsti",
                table: "Suject");

            migrationBuilder.AlterColumn<int>(
                name: "IdEducationalInsti",
                table: "Suject",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Suject_EducationalInstitution_IdEducationalInsti",
                table: "Suject",
                column: "IdEducationalInsti",
                principalTable: "EducationalInstitution",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Suject_EducationalInstitution_IdEducationalInsti",
                table: "Suject");

            migrationBuilder.AlterColumn<int>(
                name: "IdEducationalInsti",
                table: "Suject",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Suject_EducationalInstitution_IdEducationalInsti",
                table: "Suject",
                column: "IdEducationalInsti",
                principalTable: "EducationalInstitution",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
