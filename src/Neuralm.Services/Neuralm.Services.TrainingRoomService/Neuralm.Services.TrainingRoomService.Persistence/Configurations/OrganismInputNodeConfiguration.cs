﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neuralm.Services.TrainingRoomService.Domain;

namespace Neuralm.Services.TrainingRoomService.Persistence.Configurations
{
    /// <summary>
    /// Represents the <see cref="OrganismInputNodeConfiguration"/> class used to configure the relations and columns in the <see cref="DbSet{TEntity}"/> for <see cref="OrganismInputNode"/> in the DbContext.
    /// </summary>
    public class OrganismInputNodeConfiguration : IEntityTypeConfiguration<OrganismInputNode>
    {
        /// <inheritdoc cref="IEntityTypeConfiguration{TEntity}.Configure"/>
        public void Configure(EntityTypeBuilder<OrganismInputNode> builder)
        {
            builder.Ignore(p => p.Id);
            builder.HasKey(sc => new { sc.OrganismId, sc.InputNodeId });
            builder
                .HasOne(p => p.Organism)
                .WithMany(p => p.Inputs)
                .HasForeignKey(p => p.OrganismId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(p => p.InputNode)
                .WithMany(p => p.OrganismInputNodes)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
