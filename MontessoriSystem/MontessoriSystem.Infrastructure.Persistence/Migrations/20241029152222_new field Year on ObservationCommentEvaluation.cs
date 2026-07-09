using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MontessoriSystem.Infrastructure.Persistence.Migrations
{
    public partial class newfieldYearonObservationCommentEvaluation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "ObservationCommentEvaluation",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Year",
                table: "ObservationCommentEvaluation");
        }
    }
}
