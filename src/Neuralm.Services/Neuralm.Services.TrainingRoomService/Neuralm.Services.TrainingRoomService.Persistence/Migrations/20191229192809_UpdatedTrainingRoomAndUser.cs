using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Neuralm.Services.TrainingRoomService.Persistence.Migrations
{
    public partial class UpdatedTrainingRoomAndUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainer_TrainingRoom_TrainingRoomId",
                table: "Trainer");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingRoomSettings_TrainingRoom_TrainingRoomId",
                table: "TrainingRoomSettings");

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "TrainingRoom",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_Trainer_TrainingRoom_TrainingRoomId",
                table: "Trainer",
                column: "TrainingRoomId",
                principalTable: "TrainingRoom",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingRoomSettings_TrainingRoom_TrainingRoomId",
                table: "TrainingRoomSettings",
                column: "TrainingRoomId",
                principalTable: "TrainingRoom",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainer_TrainingRoom_TrainingRoomId",
                table: "Trainer");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingRoomSettings_TrainingRoom_TrainingRoomId",
                table: "TrainingRoomSettings");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "TrainingRoom");

            migrationBuilder.AddForeignKey(
                name: "FK_Trainer_TrainingRoom_TrainingRoomId",
                table: "Trainer",
                column: "TrainingRoomId",
                principalTable: "TrainingRoom",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingRoomSettings_TrainingRoom_TrainingRoomId",
                table: "TrainingRoomSettings",
                column: "TrainingRoomId",
                principalTable: "TrainingRoom",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
