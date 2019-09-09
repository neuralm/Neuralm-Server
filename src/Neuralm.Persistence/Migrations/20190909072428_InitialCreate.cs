using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Neuralm.Persistence.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CredentialType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 32, nullable: false),
                    Name = table.Column<string>(maxLength: 64, nullable: false),
                    Position = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CredentialType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Node",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Layer = table.Column<uint>(nullable: false),
                    NodeIdentifier = table.Column<uint>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Node", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 32, nullable: false),
                    Name = table.Column<string>(maxLength: 64, nullable: false),
                    Position = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 32, nullable: false),
                    Name = table.Column<string>(maxLength: 64, nullable: false),
                    Position = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Username = table.Column<string>(maxLength: 64, nullable: false),
                    TimestampCreated = table.Column<DateTime>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RolePermission",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false),
                    PermissionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermission", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_RolePermission_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermission_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Credential",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    CredentialTypeId = table.Column<int>(nullable: false),
                    Identifier = table.Column<string>(maxLength: 64, nullable: false),
                    Secret = table.Column<string>(maxLength: 1024, nullable: true),
                    Extra = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credential", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Credential_CredentialType_CredentialTypeId",
                        column: x => x.CredentialTypeId,
                        principalTable: "CredentialType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Credential_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingRoom",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OwnerId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Generation = table.Column<uint>(nullable: false),
                    HighestInnovationNumber = table.Column<uint>(nullable: false),
                    Enabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingRoom", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingRoom_User_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false),
                    UserId1 = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_User_UserId1",
                        column: x => x.UserId1,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    table.ForeignKey(
                        name: "FK_Trainer_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingRoomSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OrganismCount = table.Column<uint>(nullable: false),
                    InputCount = table.Column<uint>(nullable: false),
                    OutputCount = table.Column<uint>(nullable: false),
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
                    Seed = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingRoomSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingRoomSettings_TrainingRoom_Id",
                        column: x => x.Id,
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
                    Generation = table.Column<uint>(nullable: false),
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
                    TrainingSessionId = table.Column<Guid>(nullable: false)
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
                    InNodeIdentifier = table.Column<uint>(nullable: false),
                    OutNodeIdentifier = table.Column<uint>(nullable: false),
                    InnovationNumber = table.Column<uint>(nullable: false),
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

            migrationBuilder.InsertData(
                table: "CredentialType",
                columns: new[] { "Id", "Code", "Name", "Position" },
                values: new object[] { 1, "Name", "Name", 1 });

            migrationBuilder.CreateIndex(
                name: "IX_ConnectionGene_OrganismId",
                table: "ConnectionGene",
                column: "OrganismId");

            migrationBuilder.CreateIndex(
                name: "IX_Credential_CredentialTypeId",
                table: "Credential",
                column: "CredentialTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Credential_UserId",
                table: "Credential",
                column: "UserId");

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
                name: "IX_RolePermission_PermissionId",
                table: "RolePermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Species_TrainingRoomId",
                table: "Species",
                column: "TrainingRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Trainer_UserId",
                table: "Trainer",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingRoom_OwnerId",
                table: "TrainingRoom",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingSession_TrainingRoomId",
                table: "TrainingSession",
                column: "TrainingRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_UserId1",
                table: "UserRole",
                column: "UserId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConnectionGene");

            migrationBuilder.DropTable(
                name: "Credential");

            migrationBuilder.DropTable(
                name: "LeasedOrganism");

            migrationBuilder.DropTable(
                name: "OrganismInputNode");

            migrationBuilder.DropTable(
                name: "OrganismOutputNode");

            migrationBuilder.DropTable(
                name: "RolePermission");

            migrationBuilder.DropTable(
                name: "Trainer");

            migrationBuilder.DropTable(
                name: "TrainingRoomSettings");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "CredentialType");

            migrationBuilder.DropTable(
                name: "TrainingSession");

            migrationBuilder.DropTable(
                name: "Organism");

            migrationBuilder.DropTable(
                name: "Node");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Species");

            migrationBuilder.DropTable(
                name: "TrainingRoom");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
