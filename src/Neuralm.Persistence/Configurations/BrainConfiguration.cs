﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neuralm.Domain.Entities.NEAT;

namespace Neuralm.Persistence.Configurations
{
    internal class BrainConfiguration : IEntityTypeConfiguration<Brain>
    {
        public void Configure(EntityTypeBuilder<Brain> builder)
        {
            builder.HasKey(p => p.Id);
            builder.OwnsMany(p => p.Genes, c => c.HasForeignKey(child => child.BrainId));
        }
    }
}