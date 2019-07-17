using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neuralm.Domain.Entities.NEAT;

namespace Neuralm.Persistence.Configurations
{
    /// <summary>
    /// The TrainingRoomSettingsConfiguration class; used to configure the relations and columns in the <see cref="DbSet{TEntity}"/> for <see cref="TrainingRoomSettings"/> in the DbContext.
    /// </summary>
    internal class TrainingRoomSettingsConfiguration : IEntityTypeConfiguration<TrainingRoomSettings>
    {
        /// <inheritdoc cref="IEntityTypeConfiguration{TEntity}.Configure"/>
        public void Configure(EntityTypeBuilder<TrainingRoomSettings> builder)
        {
            builder.HasKey(p => p.Id);
        }
    }
}
