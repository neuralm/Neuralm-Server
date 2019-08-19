﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neuralm.Domain.Entities.NEAT;

namespace Neuralm.Persistence.Configurations
{
    /// <summary>
    /// Represents the <see cref="OrganismOutputNodeConfiguration"/> class used to configure the relations and columns in the <see cref="DbSet{TEntity}"/> for <see cref="OrganismOutputNode"/> in the DbContext.
    /// </summary>
    public class OrganismOutputNodeConfiguration : IEntityTypeConfiguration<OrganismOutputNode>
    {
        /// <inheritdoc cref="IEntityTypeConfiguration{TEntity}.Configure"/>
        public void Configure(EntityTypeBuilder<OrganismOutputNode> builder)
        {
            builder.HasKey(sc => new { sc.OrganismId, sc.OutputNodeId });
            builder
                .HasOne(p => p.Organism)
                .WithMany(p => p.Outputs)
                .HasForeignKey(p => p.OrganismId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
