using Microsoft.EntityFrameworkCore.Migrations;

namespace Neuralm.Services.TrainingRoomService.Persistence.Migrations
{
    public partial class ChampionSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ChampionCloneMinSpeciesSize",
                table: "TrainingRoomSettings",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChampionCloneMinSpeciesSize",
                table: "TrainingRoomSettings");
        }
    }
}
