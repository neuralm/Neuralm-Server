﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Neuralm.Services.TrainingRoomService.Persistence.Contexts;

namespace Neuralm.Services.TrainingRoomService.Persistence.Migrations
{
    [DbContext(typeof(TrainingRoomDbContext))]
    [Migration("20200201205148_Added_Scoring")]
    partial class Added_Scoring
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Neuralm.Services.TrainingRoomService.Domain.ConnectionGene", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Enabled")
                        .HasColumnType("bit");

                    b.Property<long>("InNodeIdentifier")
                        .HasColumnType("bigint");

                    b.Property<long>("InnovationNumber")
                        .HasColumnType("bigint");

                    b.Property<Guid>("OrganismId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("OutNodeIdentifier")
                        .HasColumnType("bigint");

                    b.Property<double>("Weight")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("OrganismId");

                    b.ToTable("ConnectionGene");
                });

            modelBuilder.Entity("Neuralm.Services.TrainingRoomService.Domain.LeasedOrganism", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LeaseEnd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LeaseStart")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("OrganismId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TrainingSessionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("OrganismId")
                        .IsUnique()
                        .HasFilter("[OrganismId] IS NOT NULL");

                    b.HasIndex("TrainingSessionId");

                    b.ToTable("LeasedOrganism");
                });

            modelBuilder.Entity("Neuralm.Services.TrainingRoomService.Domain.Node", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Layer")
                        .HasColumnType("bigint");

                    b.Property<long>("NodeIdentifier")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Node");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Node");
                });

            modelBuilder.Entity("Neuralm.Services.TrainingRoomService.Domain.Organism", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("Generation")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsEvaluated")
                        .HasColumnType("bit");

                    b.Property<bool>("IsLeased")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Score")
                        .HasColumnType("float");

                    b.Property<Guid>("SpeciesId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SpeciesId");

                    b.ToTable("Organism");
                });

            modelBuilder.Entity("Neuralm.Services.TrainingRoomService.Domain.OrganismInputNode", b =>
                {
                    b.Property<Guid>("OrganismId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("InputNodeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("OrganismId", "InputNodeId");

                    b.HasIndex("InputNodeId");

                    b.ToTable("OrganismInputNode");
                });

            modelBuilder.Entity("Neuralm.Services.TrainingRoomService.Domain.OrganismOutputNode", b =>
                {
                    b.Property<Guid>("OrganismId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("OutputNodeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("OrganismId", "OutputNodeId");

                    b.HasIndex("OutputNodeId");

                    b.ToTable("OrganismOutputNode");
                });

            modelBuilder.Entity("Neuralm.Services.TrainingRoomService.Domain.Species", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("SpeciesScore")
                        .HasColumnType("float");

                    b.Property<Guid>("TrainingRoomId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TrainingRoomId");

                    b.ToTable("Species");
                });

            modelBuilder.Entity("Neuralm.Services.TrainingRoomService.Domain.TrainingRoom", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Enabled")
                        .HasColumnType("bit");

                    b.Property<long>("Generation")
                        .HasColumnType("bigint");

                    b.Property<long>("HighestInnovationNumber")
                        .HasColumnType("bigint");

                    b.Property<double>("HighestOrganismScore")
                        .HasColumnType("float");

                    b.Property<double>("LowestOrganismScore")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("TotalScore")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("TrainingRoom");
                });

            modelBuilder.Entity("Neuralm.Services.TrainingRoomService.Domain.TrainingSession", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("EndedTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartedTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("TrainingRoomId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TrainingRoomId");

                    b.ToTable("TrainingSession");
                });

            modelBuilder.Entity("Neuralm.Services.TrainingRoomService.Domain.InputNode", b =>
                {
                    b.HasBaseType("Neuralm.Services.TrainingRoomService.Domain.Node");

                    b.HasDiscriminator().HasValue("InputNode");
                });

            modelBuilder.Entity("Neuralm.Services.TrainingRoomService.Domain.OutputNode", b =>
                {
                    b.HasBaseType("Neuralm.Services.TrainingRoomService.Domain.Node");

                    b.HasDiscriminator().HasValue("OutputNode");
                });

            modelBuilder.Entity("Neuralm.Services.TrainingRoomService.Domain.ConnectionGene", b =>
                {
                    b.HasOne("Neuralm.Services.TrainingRoomService.Domain.Organism", null)
                        .WithMany("ConnectionGenes")
                        .HasForeignKey("OrganismId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Neuralm.Services.TrainingRoomService.Domain.LeasedOrganism", b =>
                {
                    b.HasOne("Neuralm.Services.TrainingRoomService.Domain.Organism", "Organism")
                        .WithOne()
                        .HasForeignKey("Neuralm.Services.TrainingRoomService.Domain.LeasedOrganism", "OrganismId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("Neuralm.Services.TrainingRoomService.Domain.TrainingSession", null)
                        .WithMany("LeasedOrganisms")
                        .HasForeignKey("TrainingSessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Neuralm.Services.TrainingRoomService.Domain.Organism", b =>
                {
                    b.HasOne("Neuralm.Services.TrainingRoomService.Domain.Species", null)
                        .WithMany("Organisms")
                        .HasForeignKey("SpeciesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Neuralm.Services.TrainingRoomService.Domain.OrganismInputNode", b =>
                {
                    b.HasOne("Neuralm.Services.TrainingRoomService.Domain.InputNode", "InputNode")
                        .WithMany("OrganismInputNodes")
                        .HasForeignKey("InputNodeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Neuralm.Services.TrainingRoomService.Domain.Organism", "Organism")
                        .WithMany("Inputs")
                        .HasForeignKey("OrganismId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Neuralm.Services.TrainingRoomService.Domain.OrganismOutputNode", b =>
                {
                    b.HasOne("Neuralm.Services.TrainingRoomService.Domain.Organism", "Organism")
                        .WithMany("Outputs")
                        .HasForeignKey("OrganismId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Neuralm.Services.TrainingRoomService.Domain.OutputNode", "OutputNode")
                        .WithMany("OrganismOutputNodes")
                        .HasForeignKey("OutputNodeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Neuralm.Services.TrainingRoomService.Domain.Species", b =>
                {
                    b.HasOne("Neuralm.Services.TrainingRoomService.Domain.TrainingRoom", null)
                        .WithMany("Species")
                        .HasForeignKey("TrainingRoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Neuralm.Services.TrainingRoomService.Domain.TrainingRoom", b =>
                {
                    b.OwnsMany("Neuralm.Services.TrainingRoomService.Domain.Trainer", "AuthorizedTrainers", b1 =>
                        {
                            b1.Property<Guid>("TrainingRoomId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("UserId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uniqueidentifier");

                            b1.HasKey("TrainingRoomId", "UserId");

                            b1.ToTable("Trainer");

                            b1.WithOwner("TrainingRoom")
                                .HasForeignKey("TrainingRoomId");
                        });

                    b.OwnsOne("Neuralm.Services.TrainingRoomService.Domain.TrainingRoomSettings", "TrainingRoomSettings", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uniqueidentifier");

                            b1.Property<double>("AddConnectionChance")
                                .HasColumnType("float");

                            b1.Property<double>("AddNodeChance")
                                .HasColumnType("float");

                            b1.Property<double>("CrossOverChance")
                                .HasColumnType("float");

                            b1.Property<double>("EnableConnectionChance")
                                .HasColumnType("float");

                            b1.Property<long>("InputCount")
                                .HasColumnType("bigint");

                            b1.Property<double>("InterSpeciesChance")
                                .HasColumnType("float");

                            b1.Property<double>("MutateWeightChance")
                                .HasColumnType("float");

                            b1.Property<double>("MutationChance")
                                .HasColumnType("float");

                            b1.Property<long>("OrganismCount")
                                .HasColumnType("bigint");

                            b1.Property<long>("OutputCount")
                                .HasColumnType("bigint");

                            b1.Property<int>("Seed")
                                .HasColumnType("int");

                            b1.Property<double>("SpeciesAverageWeightDiffWeight")
                                .HasColumnType("float");

                            b1.Property<double>("SpeciesDisjointGeneWeight")
                                .HasColumnType("float");

                            b1.Property<double>("SpeciesExcessGeneWeight")
                                .HasColumnType("float");

                            b1.Property<double>("Threshold")
                                .HasColumnType("float");

                            b1.Property<double>("TopAmountToSurvive")
                                .HasColumnType("float");

                            b1.Property<Guid>("TrainingRoomId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<double>("WeightReassignChance")
                                .HasColumnType("float");

                            b1.HasKey("Id");

                            b1.HasIndex("TrainingRoomId")
                                .IsUnique();

                            b1.ToTable("TrainingRoomSettings");

                            b1.WithOwner()
                                .HasForeignKey("TrainingRoomId");
                        });
                });

            modelBuilder.Entity("Neuralm.Services.TrainingRoomService.Domain.TrainingSession", b =>
                {
                    b.HasOne("Neuralm.Services.TrainingRoomService.Domain.TrainingRoom", "TrainingRoom")
                        .WithMany("TrainingSessions")
                        .HasForeignKey("TrainingRoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
