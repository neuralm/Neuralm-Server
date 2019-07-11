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
                    BrainCount = table.Column<long>(nullable: false),
                    InputCount = table.Column<long>(nullable: false),
                    OutputCount = table.Column<long>(nullable: false),
                    C1 = table.Column<double>(nullable: false),
                    C2 = table.Column<double>(nullable: false),
                    C3 = table.Column<double>(nullable: false),
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
                name: "ConnectionGene",
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
                    table.PrimaryKey("PK_ConnectionGene", x => x.Id);
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
                });

            migrationBuilder.CreateTable(
                name: "Brain",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Score = table.Column<double>(nullable: false),
                    TrainingRoomId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brain", x => x.Id);
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
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Username = table.Column<string>(maxLength: 64, nullable: false),
                    TimestampCreated = table.Column<DateTime>(nullable: false, defaultValueSql: "GetDate()"),
                    TrainingRoomId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
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

            migrationBuilder.InsertData(
                table: "CredentialTypes",
                columns: new[] { "Id", "Code", "Name", "Position" },
                values: new object[] { 1, "Name", "Name", 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Brain_TrainingRoomId",
                table: "Brain",
                column: "TrainingRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectionGene_BrainId",
                table: "ConnectionGene",
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
                name: "IX_RolePermissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingRooms_OwnerId",
                table: "TrainingRooms",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingRooms_TrainingRoomSettingsId",
                table: "TrainingRooms",
                column: "TrainingRoomSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingSession_TrainingRoomId",
                table: "TrainingSession",
                column: "TrainingRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId1",
                table: "UserRoles",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TrainingRoomId",
                table: "Users",
                column: "TrainingRoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConnectionGene_Brain_BrainId",
                table: "ConnectionGene",
                column: "BrainId",
                principalTable: "Brain",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Credentials_Users_UserId",
                table: "Credentials",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Users_UserId1",
                table: "UserRoles",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Brain_TrainingRooms_TrainingRoomId",
                table: "Brain",
                column: "TrainingRoomId",
                principalTable: "TrainingRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingSession_TrainingRooms_TrainingRoomId",
                table: "TrainingSession",
                column: "TrainingRoomId",
                principalTable: "TrainingRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_TrainingRooms_TrainingRoomId",
                table: "Users",
                column: "TrainingRoomId",
                principalTable: "TrainingRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_TrainingRooms_TrainingRoomId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "ConnectionGene");

            migrationBuilder.DropTable(
                name: "Credentials");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "TrainingSession");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Brain");

            migrationBuilder.DropTable(
                name: "CredentialTypes");

            migrationBuilder.DropTable(
                name: "Permissions");

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
