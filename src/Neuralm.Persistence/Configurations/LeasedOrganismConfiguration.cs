using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neuralm.Domain.Entities.NEAT;

namespace Neuralm.Persistence.Configurations
{
    /// <summary>
    /// Represents the <see cref="LeasedOrganismConfiguration"/> class used to configure the relations and columns in the <see cref="DbSet{TEntity}"/> for <see cref="Brain"/> in the DbContext.
    /// </summary>
    internal class LeasedOrganismConfiguration : IEntityTypeConfiguration<LeasedOrganism>
    {
        /// <inheritdoc cref="IEntityTypeConfiguration{TEntity}.Configure"/>
        public void Configure(EntityTypeBuilder<LeasedOrganism> builder)
        {
            builder.HasKey(p => p.Id);

            // NOTE: ???
            builder.Ignore(p => p.Organism);
        }
    }
}
