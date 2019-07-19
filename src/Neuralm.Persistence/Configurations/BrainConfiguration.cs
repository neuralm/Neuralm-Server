﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neuralm.Domain.Entities.NEAT;

namespace Neuralm.Persistence.Configurations
{
    /// <summary>
    /// Represents the <see cref="BrainConfiguration"/> class used to configure the relations and columns in the <see cref="DbSet{TEntity}"/> for <see cref="Brain"/> in the DbContext.
    /// </summary>
    internal class BrainConfiguration : IEntityTypeConfiguration<Brain>
    {
        /// <inheritdoc cref="IEntityTypeConfiguration{TEntity}.Configure"/>
        public void Configure(EntityTypeBuilder<Brain> builder)
        {
            builder.HasKey(p => p.Id);
            builder.OwnsMany(p => p.ConnectionGenes)
                .HasForeignKey(cg => cg.BrainId);
            builder.Metadata.FindNavigation(nameof(Brain.ConnectionGenes))
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.Ignore(p => p.TrainingRoom);
            builder.Ignore(p => p.ConnectionGenes);
        }
    }
}
