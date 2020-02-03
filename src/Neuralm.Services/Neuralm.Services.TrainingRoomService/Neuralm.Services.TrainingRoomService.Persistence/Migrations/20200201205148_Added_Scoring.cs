using Microsoft.EntityFrameworkCore.Migrations;

namespace Neuralm.Services.TrainingRoomService.Persistence.Migrations
{
    public partial class Added_Scoring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "HighestOrganismScore",
                table: "TrainingRoom",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LowestOrganismScore",
                table: "TrainingRoom",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalScore",
                table: "TrainingRoom",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HighestOrganismScore",
                table: "TrainingRoom");

            migrationBuilder.DropColumn(
                name: "LowestOrganismScore",
                table: "TrainingRoom");

            migrationBuilder.DropColumn(
                name: "TotalScore",
                table: "TrainingRoom");
        }
    }
}
