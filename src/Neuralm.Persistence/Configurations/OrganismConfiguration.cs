using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neuralm.Domain.Entities.NEAT;

namespace Neuralm.Persistence.Configurations
{
    /// <summary>
    /// Represents the <see cref="OrganismConfiguration"/> class used to configure the relations and columns in the <see cref="DbSet{TEntity}"/> for <see cref="Organism"/> in the DbContext.
    /// </summary>
    internal class OrganismConfiguration : IEntityTypeConfiguration<Organism>
    {
        /// <inheritdoc cref="IEntityTypeConfiguration{TEntity}.Configure"/>
        public void Configure(EntityTypeBuilder<Organism> builder)
        {
            builder.HasKey(p => p.Id);
            //builder.OwnsOne(p => p.Brain)
            //    .HasForeignKey(p => p.Id);
        }
    }
}
