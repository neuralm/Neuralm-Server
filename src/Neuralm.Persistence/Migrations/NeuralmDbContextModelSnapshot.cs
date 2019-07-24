﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Neuralm.Persistence.Contexts;

namespace Neuralm.Persistence.Migrations
{
    [DbContext(typeof(NeuralmDbContext))]
    partial class NeuralmDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Neuralm.Domain.Entities.Authentication.Credential", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CredentialTypeId");

                    b.Property<string>("Extra");

                    b.Property<string>("Identifier")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<string>("Secret")
                        .HasMaxLength(1024);

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("CredentialTypeId");

                    b.HasIndex("UserId");

                    b.ToTable("Credentials");
                });

            modelBuilder.Entity("Neuralm.Domain.Entities.Authentication.CredentialType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<int?>("Position");

                    b.HasKey("Id");

                    b.ToTable("CredentialTypes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Code = "Name",
                            Name = "Name",
                            Position = 1
                        });
                });

            modelBuilder.Entity("Neuralm.Domain.Entities.Authentication.Permission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<int?>("Position");

                    b.HasKey("Id");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("Neuralm.Domain.Entities.Authentication.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<int?>("Position");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Neuralm.Domain.Entities.Authentication.RolePermission", b =>
                {
                    b.Property<int>("RoleId");

                    b.Property<int>("PermissionId");

                    b.HasKey("RoleId", "PermissionId");

                    b.HasIndex("PermissionId");

                    b.ToTable("RolePermissions");
                });

            modelBuilder.Entity("Neuralm.Domain.Entities.Authentication.UserRole", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("RoleId");

                    b.Property<Guid?>("UserId1");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId1");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("Neuralm.Domain.Entities.NEAT.Brain", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("OrganismId");

                    b.Property<Guid>("TrainingRoomId");

                    b.HasKey("Id");

                    b.HasIndex("TrainingRoomId");

                    b.ToTable("Brains");
                });

            modelBuilder.Entity("Neuralm.Domain.Entities.NEAT.TrainingRoom", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("AverageScore");

                    b.Property<bool>("Enabled");

                    b.Property<long>("Generation");

                    b.Property<double>("HighestScore");

                    b.Property<long>("InnovationId");

                    b.Property<double>("LowestScore");

                    b.Property<string>("Name");

                    b.Property<Guid>("OwnerId");

                    b.Property<Guid?>("TrainingRoomSettingsId");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("TrainingRoomSettingsId");

                    b.ToTable("TrainingRooms");
                });

            modelBuilder.Entity("Neuralm.Domain.Entities.NEAT.TrainingRoomSettings", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("AddConnectionChance");

                    b.Property<double>("AddNodeChance");

                    b.Property<double>("CrossOverChance");

                    b.Property<double>("EnableConnectionChance");

                    b.Property<long>("InputCount");

                    b.Property<double>("InterSpeciesChance");

                    b.Property<double>("MutateWeightChance");

                    b.Property<double>("MutationChance");

                    b.Property<long>("OrganismCount");

                    b.Property<long>("OutputCount");

                    b.Property<int>("Seed");

                    b.Property<double>("SpeciesAverageWeightDiffWeight");

                    b.Property<double>("SpeciesDisjointGeneWeight");

                    b.Property<double>("SpeciesExcessGeneWeight");

                    b.Property<double>("Threshold");

                    b.Property<double>("TopAmountToSurvive");

                    b.Property<double>("WeightReassignChance");

                    b.HasKey("Id");

                    b.ToTable("TrainingRoomSettings");
                });

            modelBuilder.Entity("Neuralm.Domain.Entities.NEAT.TrainingSession", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("EndedTimestamp");

                    b.Property<DateTime>("StartedTimestamp");

                    b.Property<Guid>("TrainingRoomId");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("TrainingRoomId");

                    b.ToTable("TrainingSessions");
                });

            modelBuilder.Entity("Neuralm.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("TimestampCreated")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GetDate()");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Neuralm.Domain.Entities.Authentication.Credential", b =>
                {
                    b.HasOne("Neuralm.Domain.Entities.Authentication.CredentialType", "CredentialType")
                        .WithMany("Credentials")
                        .HasForeignKey("CredentialTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Neuralm.Domain.Entities.User", "User")
                        .WithMany("Credentials")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Neuralm.Domain.Entities.Authentication.RolePermission", b =>
                {
                    b.HasOne("Neuralm.Domain.Entities.Authentication.Permission", "Permission")
                        .WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Neuralm.Domain.Entities.Authentication.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Neuralm.Domain.Entities.Authentication.UserRole", b =>
                {
                    b.HasOne("Neuralm.Domain.Entities.Authentication.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Neuralm.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId1");
                });

            modelBuilder.Entity("Neuralm.Domain.Entities.NEAT.Brain", b =>
                {
                    b.HasOne("Neuralm.Domain.Entities.NEAT.TrainingRoom", "TrainingRoom")
                        .WithMany("Brains")
                        .HasForeignKey("TrainingRoomId");

                    b.OwnsMany("Neuralm.Domain.Entities.NEAT.ConnectionGene", "ConnectionGenes", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd();

                            b1.Property<Guid>("BrainId");

                            b1.Property<bool>("Enabled");

                            b1.Property<long>("InId");

                            b1.Property<long>("InnovationNumber");

                            b1.Property<long>("OutId");

                            b1.Property<double>("Weight");

                            b1.HasKey("Id");

                            b1.HasIndex("BrainId");

                            b1.ToTable("ConnectionGenes");

                            b1.HasOne("Neuralm.Domain.Entities.NEAT.Brain")
                                .WithMany("ConnectionGenes")
                                .HasForeignKey("BrainId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("Neuralm.Domain.Entities.NEAT.TrainingRoom", b =>
                {
                    b.HasOne("Neuralm.Domain.Entities.User", "Owner")
                        .WithMany("TrainingRooms")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Neuralm.Domain.Entities.NEAT.TrainingRoomSettings", "TrainingRoomSettings")
                        .WithMany()
                        .HasForeignKey("TrainingRoomSettingsId");

                    b.OwnsMany("Neuralm.Domain.Entities.NEAT.Species", "Species", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd();

                            b1.Property<double>("SpeciesScore");

                            b1.Property<Guid>("TrainingRoomId");

                            b1.HasKey("Id");

                            b1.HasIndex("TrainingRoomId");

                            b1.ToTable("Species");

                            b1.HasOne("Neuralm.Domain.Entities.NEAT.TrainingRoom")
                                .WithMany("Species")
                                .HasForeignKey("TrainingRoomId");

                            b1.OwnsMany("Neuralm.Domain.Entities.NEAT.Organism", "LastGenerationOrganisms", b2 =>
                                {
                                    b2.Property<Guid>("Id")
                                        .ValueGeneratedOnAdd();

                                    b2.Property<Guid>("BrainId");

                                    b2.Property<long>("Generation");

                                    b2.Property<string>("Name");

                                    b2.Property<double>("Score");

                                    b2.Property<Guid>("SpeciesId");

                                    b2.Property<Guid>("TrainingRoomId");

                                    b2.HasKey("Id");

                                    b2.HasIndex("BrainId");

                                    b2.HasIndex("SpeciesId");

                                    b2.HasIndex("TrainingRoomId");

                                    b2.ToTable("Species_LastGenerationOrganisms");

                                    b2.HasOne("Neuralm.Domain.Entities.NEAT.Brain", "Brain")
                                        .WithMany()
                                        .HasForeignKey("BrainId")
                                        .OnDelete(DeleteBehavior.Cascade);

                                    b2.HasOne("Neuralm.Domain.Entities.NEAT.Species")
                                        .WithMany("LastGenerationOrganisms")
                                        .HasForeignKey("SpeciesId");

                                    b2.HasOne("Neuralm.Domain.Entities.NEAT.TrainingRoom", "TrainingRoom")
                                        .WithMany()
                                        .HasForeignKey("TrainingRoomId")
                                        .OnDelete(DeleteBehavior.Cascade);
                                });
                        });

                    b.OwnsMany("Neuralm.Domain.Entities.NEAT.Organism", "Organisms", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd();

                            b1.Property<Guid>("BrainId");

                            b1.Property<long>("Generation");

                            b1.Property<string>("Name");

                            b1.Property<double>("Score");

                            b1.Property<Guid>("SpeciesId");

                            b1.Property<Guid>("TrainingRoomId");

                            b1.HasKey("Id");

                            b1.HasIndex("BrainId");

                            b1.HasIndex("TrainingRoomId");

                            b1.ToTable("TrainingRooms_Organisms");

                            b1.HasOne("Neuralm.Domain.Entities.NEAT.Brain", "Brain")
                                .WithMany()
                                .HasForeignKey("BrainId")
                                .OnDelete(DeleteBehavior.Cascade);

                            b1.HasOne("Neuralm.Domain.Entities.NEAT.TrainingRoom", "TrainingRoom")
                                .WithMany("Organisms")
                                .HasForeignKey("TrainingRoomId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("Neuralm.Domain.Entities.NEAT.TrainingSession", b =>
                {
                    b.HasOne("Neuralm.Domain.Entities.NEAT.TrainingRoom", "TrainingRoom")
                        .WithMany("TrainingSessions")
                        .HasForeignKey("TrainingRoomId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
