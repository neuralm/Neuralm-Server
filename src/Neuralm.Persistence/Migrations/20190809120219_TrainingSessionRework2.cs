using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Neuralm.Persistence.Migrations
{
    public partial class TrainingSessionRework2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CredentialTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 32, nullable: false),
                    Name = table.Column<string>(maxLength: 64, nullable: false),
                    Position = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CredentialTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 32, nullable: false),
                    Name = table.Column<string>(maxLength: 64, nullable: false),
                    Position = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 32, nullable: false),
                    Name = table.Column<string>(maxLength: 64, nullable: false),
                    Position = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
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
                    Seed = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingRoomSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Username = table.Column<string>(maxLength: 64, nullable: false),
                    TimestampCreated = table.Column<DateTime>(nullable: false, defaultValueSql: "GetDate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false),
                    PermissionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Credentials",
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
                    table.PrimaryKey("PK_Credentials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Credentials_CredentialTypes_CredentialTypeId",
                        column: x => x.CredentialTypeId,
                        principalTable: "CredentialTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Credentials_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingRooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OwnerId = table.Column<Guid>(nullable: false),
                    TrainingRoomSettingsId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Generation = table.Column<long>(nullable: false),
                    HighestScore = table.Column<double>(nullable: false),
                    LowestScore = table.Column<double>(nullable: false),
                    AverageScore = table.Column<double>(nullable: false),
                    InnovationId = table.Column<long>(nullable: false),
                    Enabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingRooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingRooms_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainingRooms_TrainingRoomSettings_TrainingRoomSettingsId",
                        column: x => x.TrainingRoomSettingsId,
                        principalTable: "TrainingRoomSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false),
                    UserId1 = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId1",
                        column: x => x.UserId1,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Brains",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TrainingRoomId = table.Column<Guid>(nullable: false),
                    OrganismId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brains", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Brains_TrainingRooms_TrainingRoomId",
                        column: x => x.TrainingRoomId,
                        principalTable: "TrainingRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Species",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TrainingRoomId = table.Column<Guid>(nullable: false),
                    SpeciesScore = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Species", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Species_TrainingRooms_TrainingRoomId",
                        column: x => x.TrainingRoomId,
                        principalTable: "TrainingRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trainers",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    TrainingRoomId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainers", x => new { x.TrainingRoomId, x.UserId });
                    table.ForeignKey(
                        name: "FK_Trainers_TrainingRooms_TrainingRoomId",
                        column: x => x.TrainingRoomId,
                        principalTable: "TrainingRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Trainers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrainingSessions",
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
                    table.PrimaryKey("PK_TrainingSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingSessions_TrainingRooms_TrainingRoomId",
                        column: x => x.TrainingRoomId,
                        principalTable: "TrainingRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConnectionGenes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BrainId = table.Column<Guid>(nullable: false),
                    InId = table.Column<long>(nullable: false),
                    OutId = table.Column<long>(nullable: false),
                    InnovationNumber = table.Column<long>(nullable: false),
                    Weight = table.Column<double>(nullable: false),
                    Enabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectionGenes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConnectionGenes_Brains_BrainId",
                        column: x => x.BrainId,
                        principalTable: "Brains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Species_LastGenerationOrganisms",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SpeciesId = table.Column<Guid>(nullable: false),
                    BrainId = table.Column<Guid>(nullable: false),
                    TrainingRoomId = table.Column<Guid>(nullable: false),
                    Score = table.Column<double>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Generation = table.Column<long>(nullable: false),
                    Leased = table.Column<bool>(nullable: false),
                    Evaluated = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Species_LastGenerationOrganisms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Species_LastGenerationOrganisms_Brains_BrainId",
                        column: x => x.BrainId,
                        principalTable: "Brains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Species_LastGenerationOrganisms_Species_SpeciesId",
                        column: x => x.SpeciesId,
                        principalTable: "Species",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Species_LastGenerationOrganisms_TrainingRooms_TrainingRoomId",
                        column: x => x.TrainingRoomId,
                        principalTable: "TrainingRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Species_Organisms",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SpeciesId = table.Column<Guid>(nullable: false),
                    BrainId = table.Column<Guid>(nullable: false),
                    TrainingRoomId = table.Column<Guid>(nullable: false),
                    Score = table.Column<double>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Generation = table.Column<long>(nullable: false),
                    Leased = table.Column<bool>(nullable: false),
                    Evaluated = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Species_Organisms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Species_Organisms_Brains_BrainId",
                        column: x => x.BrainId,
                        principalTable: "Brains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Species_Organisms_Species_SpeciesId",
                        column: x => x.SpeciesId,
                        principalTable: "Species",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Species_Organisms_TrainingRooms_TrainingRoomId",
                        column: x => x.TrainingRoomId,
                        principalTable: "TrainingRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeasedOrganisms",
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
                    table.PrimaryKey("PK_LeasedOrganisms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeasedOrganisms_TrainingSessions_TrainingSessionId",
                        column: x => x.TrainingSessionId,
                        principalTable: "TrainingSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CredentialTypes",
                columns: new[] { "Id", "Code", "Name", "Position" },
                values: new object[] { 1, "Name", "Name", 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Brains_TrainingRoomId",
                table: "Brains",
                column: "TrainingRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectionGenes_BrainId",
                table: "ConnectionGenes",
                column: "BrainId");

            migrationBuilder.CreateIndex(
                name: "IX_Credentials_CredentialTypeId",
                table: "Credentials",
                column: "CredentialTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Credentials_UserId",
                table: "Credentials",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LeasedOrganisms_TrainingSessionId",
                table: "LeasedOrganisms",
                column: "TrainingSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Species_TrainingRoomId",
                table: "Species",
                column: "TrainingRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Species_LastGenerationOrganisms_BrainId",
                table: "Species_LastGenerationOrganisms",
                column: "BrainId");

            migrationBuilder.CreateIndex(
                name: "IX_Species_LastGenerationOrganisms_SpeciesId",
                table: "Species_LastGenerationOrganisms",
                column: "SpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_Species_LastGenerationOrganisms_TrainingRoomId",
                table: "Species_LastGenerationOrganisms",
                column: "TrainingRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Species_Organisms_BrainId",
                table: "Species_Organisms",
                column: "BrainId");

            migrationBuilder.CreateIndex(
                name: "IX_Species_Organisms_SpeciesId",
                table: "Species_Organisms",
                column: "SpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_Species_Organisms_TrainingRoomId",
                table: "Species_Organisms",
                column: "TrainingRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_UserId",
                table: "Trainers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingRooms_OwnerId",
                table: "TrainingRooms",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingRooms_TrainingRoomSettingsId",
                table: "TrainingRooms",
                column: "TrainingRoomSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingSessions_TrainingRoomId",
                table: "TrainingSessions",
                column: "TrainingRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId1",
                table: "UserRoles",
                column: "UserId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConnectionGenes");

            migrationBuilder.DropTable(
                name: "Credentials");

            migrationBuilder.DropTable(
                name: "LeasedOrganisms");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "Species_LastGenerationOrganisms");

            migrationBuilder.DropTable(
                name: "Species_Organisms");

            migrationBuilder.DropTable(
                name: "Trainers");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "CredentialTypes");

            migrationBuilder.DropTable(
                name: "TrainingSessions");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Brains");

            migrationBuilder.DropTable(
                name: "Species");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "TrainingRooms");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "TrainingRoomSettings");
        }
    }
}
