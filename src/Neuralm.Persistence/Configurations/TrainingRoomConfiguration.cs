using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neuralm.Domain.Entities.NEAT;

namespace Neuralm.Persistence.Configurations
{
    /// <summary>
    /// Represents the <see cref="TrainingRoomConfiguration"/> class used to configure the relations and columns in the <see cref="DbSet{TEntity}"/> for <see cref="TrainingRoom"/> in the DbContext.
    /// </summary>
    internal class TrainingRoomConfiguration : IEntityTypeConfiguration<TrainingRoom>
    {
        /// <inheritdoc cref="IEntityTypeConfiguration{TEntity}.Configure"/>
        public void Configure(EntityTypeBuilder<TrainingRoom> builder)
        {
            builder.HasKey(p => p.Id);
            builder.OwnsMany(p => p.Organisms)
                .HasOne(p => p.TrainingRoom);
            builder.Metadata.FindNavigation(nameof(TrainingRoom.Organisms))
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.OwnsMany(p => p.TrainingSessions)
                .HasForeignKey(p => p.TrainingRoomId);
            builder.Metadata.FindNavigation(nameof(TrainingRoom.TrainingSessions))
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.OwnsMany(p => p.Species)
                .HasForeignKey(p => p.TrainingRoomId);
            builder.Metadata.FindNavigation(nameof(TrainingRoom.Species))
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            //builder.HasMany(p => p.AuthorizedUsers);
            // NOTE: Ignore users for now...
            builder.Ignore(p => p.AuthorizedUsers);
            builder.Ignore(p => p.Random);
            //builder.Ignore(p => p.Species);
            //builder.Ignore(p => p.TrainingSessions);
            //builder.Ignore(p => p.Brains);
        }
    }
}
