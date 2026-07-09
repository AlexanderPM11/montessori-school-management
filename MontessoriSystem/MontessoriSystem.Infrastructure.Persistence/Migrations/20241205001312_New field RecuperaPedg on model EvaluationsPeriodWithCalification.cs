using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MontessoriSystem.Infrastructure.Persistence.Migrations
{
    public partial class NewfieldRecuperaPedgonmodelEvaluationsPeriodWithCalification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RecuperaPedg",
                table: "EvaluationsPeriodWithCalification",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecuperaPedg",
                table: "EvaluationsPeriodWithCalification");
        }
    }
}
