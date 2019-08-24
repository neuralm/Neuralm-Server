using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neuralm.Domain.Entities.NEAT;

namespace Neuralm.Persistence.Configurations
{
    /// <summary>
    /// Represents the <see cref="OrganismInputNodeConfiguration"/> class used to configure the relations and columns in the <see cref="DbSet{TEntity}"/> for <see cref="OrganismInputNode"/> in the DbContext.
    /// </summary>
    public class OrganismInputNodeConfiguration : IEntityTypeConfiguration<OrganismInputNode>
    {
        /// <inheritdoc cref="IEntityTypeConfiguration{TEntity}.Configure"/>
        public void Configure(EntityTypeBuilder<OrganismInputNode> builder)
        {
            builder.HasKey(sc => new { sc.OrganismId, sc.InputNodeId });
            builder
                .HasOne(p => p.Organism)
                .WithMany(p => p.Inputs)
                .HasForeignKey(p => p.OrganismId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
