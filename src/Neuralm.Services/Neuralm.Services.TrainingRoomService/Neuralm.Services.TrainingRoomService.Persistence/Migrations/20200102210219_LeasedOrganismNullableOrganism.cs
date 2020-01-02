using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Neuralm.Services.TrainingRoomService.Persistence.Migrations
{
    public partial class LeasedOrganismNullableOrganism : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainer_TrainingRoom_TrainingRoomId",
                table: "Trainer");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingRoomSettings_TrainingRoom_TrainingRoomId",
                table: "TrainingRoomSettings");

            migrationBuilder.DropIndex(
                name: "IX_LeasedOrganism_OrganismId",
                table: "LeasedOrganism");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrganismId",
                table: "LeasedOrganism",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_LeasedOrganism_OrganismId",
                table: "LeasedOrganism",
                column: "OrganismId",
                unique: true,
                filter: "[OrganismId] IS NOT NULL");

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

            migrationBuilder.DropIndex(
                name: "IX_LeasedOrganism_OrganismId",
                table: "LeasedOrganism");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrganismId",
                table: "LeasedOrganism",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeasedOrganism_OrganismId",
                table: "LeasedOrganism",
                column: "OrganismId",
                unique: true);

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
