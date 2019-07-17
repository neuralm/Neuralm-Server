using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neuralm.Domain.Entities.NEAT;

namespace Neuralm.Persistence.Configurations
{
    /// <summary>
    /// The ConnectionGeneConfiguration class; used to configure the relations and columns in the <see cref="DbSet{TEntity}"/> for <see cref="ConnectionGene"/> in the DbContext.
    /// </summary>
    internal class ConnectionGeneConfiguration : IEntityTypeConfiguration<ConnectionGene>
    {
        /// <inheritdoc cref="IEntityTypeConfiguration{TEntity}.Configure"/>
        public void Configure(EntityTypeBuilder<ConnectionGene> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Ignore(p => p.InNode);
            builder.Ignore(p => p.OutNode);
        }
    }
}
