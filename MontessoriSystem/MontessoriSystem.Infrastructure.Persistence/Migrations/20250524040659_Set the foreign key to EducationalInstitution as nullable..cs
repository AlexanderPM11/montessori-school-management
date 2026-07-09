using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MontessoriSystem.Infrastructure.Persistence.Migrations
{
    public partial class SettheforeignkeytoEducationalInstitutionasnullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Room_EducationalInstitution_IdEducationalInsti",
                table: "Room");

            migrationBuilder.AlterColumn<int>(
                name: "IdEducationalInsti",
                table: "Room",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Room_EducationalInstitution_IdEducationalInsti",
                table: "Room",
                column: "IdEducationalInsti",
                principalTable: "EducationalInstitution",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Room_EducationalInstitution_IdEducationalInsti",
                table: "Room");

            migrationBuilder.AlterColumn<int>(
                name: "IdEducationalInsti",
                table: "Room",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Room_EducationalInstitution_IdEducationalInsti",
                table: "Room",
                column: "IdEducationalInsti",
                principalTable: "EducationalInstitution",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
