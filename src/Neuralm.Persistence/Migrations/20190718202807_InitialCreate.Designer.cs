﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Neuralm.Persistence.Contexts;

namespace Neuralm.Persistence.Migrations
{
    [DbContext(typeof(NeuralmDbContext))]
    [Migration("20190718202807_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

            modelBuilder.Entity("Neuralm.Domain.Entities.User", b =>
                {
                    b.OwnsMany("Neuralm.Domain.Entities.NEAT.TrainingRoom", "TrainingRooms", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd();

                            b1.Property<double>("AverageScore");

                            b1.Property<bool>("Enabled");

                            b1.Property<long>("Generation");

                            b1.Property<double>("HighestScore");

                            b1.Property<long>("InnovationId");

                            b1.Property<double>("LowestScore");

                            b1.Property<string>("Name");

                            b1.Property<Guid>("OwnerId");

                            b1.Property<Guid?>("TrainingRoomSettingsId");

                            b1.HasKey("Id");

                            b1.HasIndex("OwnerId");

                            b1.HasIndex("TrainingRoomSettingsId");

                            b1.ToTable("TrainingRooms");

                            b1.HasOne("Neuralm.Domain.Entities.User", "Owner")
                                .WithMany("TrainingRooms")
                                .HasForeignKey("OwnerId")
                                .OnDelete(DeleteBehavior.Cascade);

                            b1.HasOne("Neuralm.Domain.Entities.NEAT.TrainingRoomSettings", "TrainingRoomSettings")
                                .WithMany()
                                .HasForeignKey("TrainingRoomSettingsId");

                            b1.OwnsMany("Neuralm.Domain.Entities.NEAT.Organism", "Organisms", b2 =>
                                {
                                    b2.Property<Guid>("Id")
                                        .ValueGeneratedOnAdd();

                                    b2.Property<Guid>("BrainId");

                                    b2.Property<double>("Score");

                                    b2.Property<Guid>("SpeciesId");

                                    b2.Property<Guid>("TrainingRoomId");

                                    b2.Property<string>("Name");

                                    b2.HasKey("Id");

                                    b2.HasIndex("TrainingRoomId");

                                    b2.ToTable("Organisms");

                                    b2.HasOne("Neuralm.Domain.Entities.NEAT.TrainingRoom", "TrainingRoom")
                                        .WithMany("Organisms")
                                        .HasForeignKey("TrainingRoomId")
                                        .OnDelete(DeleteBehavior.Cascade);

                                    b2.OwnsOne("Neuralm.Domain.Entities.NEAT.Brain", "Brain", b3 =>
                                        {
                                            b3.Property<Guid>("Id");

                                            b3.Property<Guid>("TrainingRoomId");

                                            b3.HasKey("Id");

                                            b3.ToTable("Brains");

                                            b3.HasOne("Neuralm.Domain.Entities.NEAT.Organism")
                                                .WithOne("Brain")
                                                .HasForeignKey("Neuralm.Domain.Entities.NEAT.Brain", "Id")
                                                .OnDelete(DeleteBehavior.Cascade);
                                        });
                                });

                            b1.OwnsMany("Neuralm.Domain.Entities.NEAT.Species", "Species", b2 =>
                                {
                                    b2.Property<Guid>("Id")
                                        .ValueGeneratedOnAdd();

                                    b2.Property<double>("SpeciesScore");

                                    b2.Property<Guid>("TrainingRoomId");

                                    b2.HasKey("Id");

                                    b2.HasIndex("TrainingRoomId");

                                    b2.ToTable("Species");

                                    b2.HasOne("Neuralm.Domain.Entities.NEAT.TrainingRoom")
                                        .WithMany("Species")
                                        .HasForeignKey("TrainingRoomId")
                                        .OnDelete(DeleteBehavior.Cascade);
                                });

                            b1.OwnsMany("Neuralm.Domain.Entities.NEAT.TrainingSession", "TrainingSessions", b2 =>
                                {
                                    b2.Property<Guid>("Id")
                                        .ValueGeneratedOnAdd();

                                    b2.Property<DateTime>("EndedTimestamp");

                                    b2.Property<DateTime>("StartedTimestamp");

                                    b2.Property<Guid>("TrainingRoomId");

                                    b2.Property<Guid>("UserId");

                                    b2.HasKey("Id");

                                    b2.HasIndex("TrainingRoomId");

                                    b2.ToTable("TrainingSessions");

                                    b2.HasOne("Neuralm.Domain.Entities.NEAT.TrainingRoom", "TrainingRoom")
                                        .WithMany("TrainingSessions")
                                        .HasForeignKey("TrainingRoomId")
                                        .OnDelete(DeleteBehavior.Cascade);
                                });
                        });
                });

            modelBuilder.Entity("Neuralm.Domain.Entities.NEAT.ConnectionGene", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<Guid>("BrainId");

                b.Property<bool>("Enabled");

                b.Property<long>("InId");

                b.Property<long>("InnovationNumber");

                b.Property<long>("OutId");

                b.Property<double>("Weight");

                b.HasKey("Id");

                b.ToTable("ConnectionGenes");

                b.HasOne("Neuralm.Domain.Entities.NEAT.Brain", "Brain")
                    .WithMany("ConnectionGenes")
                    .HasForeignKey("BrainId")
                    .OnDelete(DeleteBehavior.Cascade);
            });
#pragma warning restore 612, 618
        }
    }
}
