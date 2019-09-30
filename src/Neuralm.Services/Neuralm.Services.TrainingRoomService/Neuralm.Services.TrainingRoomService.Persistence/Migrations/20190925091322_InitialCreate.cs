using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Neuralm.Services.TrainingRoomService.Persistence.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Node",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Layer = table.Column<long>(nullable: false),
                    NodeIdentifier = table.Column<long>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Node", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingRoom",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Generation = table.Column<long>(nullable: false),
                    HighestInnovationNumber = table.Column<long>(nullable: false),
                    Enabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingRoom", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Species",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SpeciesScore = table.Column<double>(nullable: false),
                    TrainingRoomId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Species", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Species_TrainingRoom_TrainingRoomId",
                        column: x => x.TrainingRoomId,
                        principalTable: "TrainingRoom",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trainer",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    TrainingRoomId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainer", x => new { x.TrainingRoomId, x.UserId });
                    table.ForeignKey(
                        name: "FK_Trainer_TrainingRoom_TrainingRoomId",
                        column: x => x.TrainingRoomId,
                        principalTable: "TrainingRoom",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingRoomSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OrganismCount = table.Column<long>(nullable: false),
                    InputCount = table.Column<long>(nullable: false),
                    OutputCount = table.Column<long>(nullable: false),
                    SpeciesExcessGeneWeight = table.Column<double>(nullable: false),
                    SpeciesDisjointGeneWeight = table.Column<double>(nullable: false),
                    SpeciesAverageWeightDiffWeight = table.Column<double>(nullable: false),
                    Threshold = table.Column<double>(nullable: false),
                    AddConnectionChance = table.Column<double>(nullable: false),
                    AddNodeChance = table.Column<double>(nullable: false),
                    CrossOverChance = table.Column<double>(nullable: false),
                    InterSpeciesChance = table.Column<double>(nullable: false),
                    MutationChance = table.Column<double>(nullable: false),
                    MutateWeightChance = table.Column<double>(nullable: false),
                    WeightReassignChance = table.Column<double>(nullable: false),
                    TopAmountToSurvive = table.Column<double>(nullable: false),
                    EnableConnectionChance = table.Column<double>(nullable: false),
                    Seed = table.Column<int>(nullable: false),
                    TrainingRoomId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingRoomSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingRoomSettings_TrainingRoom_TrainingRoomId",
                        column: x => x.TrainingRoomId,
                        principalTable: "TrainingRoom",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingSession",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    StartedTimestamp = table.Column<DateTime>(nullable: false),
                    EndedTimestamp = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    TrainingRoomId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingSession", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingSession_TrainingRoom_TrainingRoomId",
                        column: x => x.TrainingRoomId,
                        principalTable: "TrainingRoom",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Organism",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Score = table.Column<double>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Generation = table.Column<long>(nullable: false),
                    Leased = table.Column<bool>(nullable: false),
                    Evaluated = table.Column<bool>(nullable: false),
                    SpeciesId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organism", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Organism_Species_SpeciesId",
                        column: x => x.SpeciesId,
                        principalTable: "Species",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeasedOrganism",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OrganismId = table.Column<Guid>(nullable: false),
                    LeaseStart = table.Column<DateTime>(nullable: false),
                    LeaseEnd = table.Column<DateTime>(nullable: false),
                    TrainingSessionId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeasedOrganism", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeasedOrganism_TrainingSession_TrainingSessionId",
                        column: x => x.TrainingSessionId,
                        principalTable: "TrainingSession",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConnectionGene",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OrganismId = table.Column<Guid>(nullable: false),
                    InNodeIdentifier = table.Column<long>(nullable: false),
                    OutNodeIdentifier = table.Column<long>(nullable: false),
                    InnovationNumber = table.Column<long>(nullable: false),
                    Weight = table.Column<double>(nullable: false),
                    Enabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectionGene", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConnectionGene_Organism_OrganismId",
                        column: x => x.OrganismId,
                        principalTable: "Organism",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrganismInputNode",
                columns: table => new
                {
                    OrganismId = table.Column<Guid>(nullable: false),
                    InputNodeId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganismInputNode", x => new { x.OrganismId, x.InputNodeId });
                    table.ForeignKey(
                        name: "FK_OrganismInputNode_Node_InputNodeId",
                        column: x => x.InputNodeId,
                        principalTable: "Node",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganismInputNode_Organism_OrganismId",
                        column: x => x.OrganismId,
                        principalTable: "Organism",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrganismOutputNode",
                columns: table => new
                {
                    OrganismId = table.Column<Guid>(nullable: false),
                    OutputNodeId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganismOutputNode", x => new { x.OrganismId, x.OutputNodeId });
                    table.ForeignKey(
                        name: "FK_OrganismOutputNode_Organism_OrganismId",
                        column: x => x.OrganismId,
                        principalTable: "Organism",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganismOutputNode_Node_OutputNodeId",
                        column: x => x.OutputNodeId,
                        principalTable: "Node",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConnectionGene_OrganismId",
                table: "ConnectionGene",
                column: "OrganismId");

            migrationBuilder.CreateIndex(
                name: "IX_LeasedOrganism_TrainingSessionId",
                table: "LeasedOrganism",
                column: "TrainingSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Organism_SpeciesId",
                table: "Organism",
                column: "SpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganismInputNode_InputNodeId",
                table: "OrganismInputNode",
                column: "InputNodeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganismOutputNode_OutputNodeId",
                table: "OrganismOutputNode",
                column: "OutputNodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Species_TrainingRoomId",
                table: "Species",
                column: "TrainingRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingRoomSettings_TrainingRoomId",
                table: "TrainingRoomSettings",
                column: "TrainingRoomId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrainingSession_TrainingRoomId",
                table: "TrainingSession",
                column: "TrainingRoomId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConnectionGene");

            migrationBuilder.DropTable(
                name: "LeasedOrganism");

            migrationBuilder.DropTable(
                name: "OrganismInputNode");

            migrationBuilder.DropTable(
                name: "OrganismOutputNode");

            migrationBuilder.DropTable(
                name: "Trainer");

            migrationBuilder.DropTable(
                name: "TrainingRoomSettings");

            migrationBuilder.DropTable(
                name: "TrainingSession");

            migrationBuilder.DropTable(
                name: "Organism");

            migrationBuilder.DropTable(
                name: "Node");

            migrationBuilder.DropTable(
                name: "Species");

            migrationBuilder.DropTable(
                name: "TrainingRoom");
        }
    }
}
