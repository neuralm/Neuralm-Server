using Microsoft.EntityFrameworkCore.Migrations;

namespace Neuralm.Services.TrainingRoomService.Persistence.Migrations
{
    public partial class AddStagnentCounter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "MaxStagnantTime",
                table: "TrainingRoomSettings",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<double>(
                name: "HighScore",
                table: "Species",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<long>(
                name: "StagnantCounter",
                table: "Species",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxStagnantTime",
                table: "TrainingRoomSettings");

            migrationBuilder.DropColumn(
                name: "HighScore",
                table: "Species");

            migrationBuilder.DropColumn(
                name: "StagnantCounter",
                table: "Species");
        }
    }
}
