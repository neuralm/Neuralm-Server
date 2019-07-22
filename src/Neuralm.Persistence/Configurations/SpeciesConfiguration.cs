using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neuralm.Domain.Entities.NEAT;

namespace Neuralm.Persistence.Configurations
{
    /// <summary>
    /// Represents the <see cref="SpeciesConfiguration"/> class used to configure the relations and columns in the <see cref="DbSet{TEntity}"/> for <see cref="Species"/> in the DbContext.
    /// </summary>
    internal class SpeciesConfiguration : IEntityTypeConfiguration<Species>
    {
        /// <inheritdoc cref="IEntityTypeConfiguration{TEntity}.Configure"/>
        public void Configure(EntityTypeBuilder<Species> builder)
        {
            builder.HasKey(p => p.Id);

            builder.OwnsMany(p => p.LastGenerationOrganisms)
                .HasForeignKey(p => p.SpeciesId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.Metadata.FindNavigation(nameof(Species.LastGenerationOrganisms))
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.Ignore(p => p.Organisms);
        }
    }
}
