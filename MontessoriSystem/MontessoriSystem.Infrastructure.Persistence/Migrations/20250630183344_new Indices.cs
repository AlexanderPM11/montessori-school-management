using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MontessoriSystem.Infrastructure.Persistence.Migrations
{
    public partial class newIndices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Attendance_IdRoom_Date",
                table: "Attendance",
                columns: new[] { "IdRoom", "Date" });

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_IdStudent_Date",
                table: "Attendance",
                columns: new[] { "IdStudent", "Date" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Attendance_IdRoom_Date",
                table: "Attendance");

            migrationBuilder.DropIndex(
                name: "IX_Attendance_IdStudent_Date",
                table: "Attendance");
        }
    }
}
