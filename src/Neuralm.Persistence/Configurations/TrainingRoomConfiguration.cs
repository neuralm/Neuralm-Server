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
            builder
                .HasKey(p => p.Id);

            builder
                .HasOne(p => p.Owner)
                .WithMany(p => p.TrainingRooms)
                .HasForeignKey(p => p.OwnerId);

            builder
                .OwnsMany(p => p.Species)
                .HasForeignKey(p => p.TrainingRoomId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder
                .OwnsMany(p => p.Organisms)
                .HasKey(p => p.Id);

            // NOTE: Ignore users for now...
            builder.Ignore(p => p.AuthorizedUsers);
            builder.Ignore(p => p.Random);
        }
    }
}
