using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neuralm.Services.TrainingRoomService.Domain;

namespace Neuralm.Services.TrainingRoomService.Persistence.Configurations
{
    /// <summary>
    /// Represents the <see cref="LeasedOrganismConfiguration"/> class used to configure the relations and columns in the <see cref="DbSet{TEntity}"/> for <see cref="Brain"/> in the DbContext.
    /// </summary>
    public class LeasedOrganismConfiguration : IEntityTypeConfiguration<LeasedOrganism>
    {
        /// <inheritdoc cref="IEntityTypeConfiguration{TEntity}.Configure"/>
        public void Configure(EntityTypeBuilder<LeasedOrganism> builder)
        {
            builder.HasKey(p => p.Id);

            builder
                .HasOne(p => p.Organism)
                .WithOne()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
