using Microsoft.EntityFrameworkCore.Migrations;

namespace Neuralm.Services.TrainingRoomService.Persistence.Migrations
{
    public partial class Addedhighestnodeidentifier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "HighestNodeIdentifier",
                table: "TrainingRoom",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HighestNodeIdentifier",
                table: "TrainingRoom");
        }
    }
}
