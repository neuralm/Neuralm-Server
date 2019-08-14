using Microsoft.EntityFrameworkCore.Migrations;

namespace Neuralm.Persistence.Migrations
{
    public partial class Check : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Species_LastGenerationOrganisms_Species_SpeciesId",
                table: "Species_LastGenerationOrganisms");

            migrationBuilder.AddForeignKey(
                name: "FK_Species_LastGenerationOrganisms_Species_SpeciesId",
                table: "Species_LastGenerationOrganisms",
                column: "SpeciesId",
                principalTable: "Species",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Species_LastGenerationOrganisms_Species_SpeciesId",
                table: "Species_LastGenerationOrganisms");

            migrationBuilder.AddForeignKey(
                name: "FK_Species_LastGenerationOrganisms_Species_SpeciesId",
                table: "Species_LastGenerationOrganisms",
                column: "SpeciesId",
                principalTable: "Species",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
