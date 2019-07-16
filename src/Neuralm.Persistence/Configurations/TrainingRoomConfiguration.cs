using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neuralm.Domain.Entities.NEAT;

namespace Neuralm.Persistence.Configurations
{
    internal class TrainingRoomConfiguration : IEntityTypeConfiguration<TrainingRoom>
    {
        public void Configure(EntityTypeBuilder<TrainingRoom> builder)
        {
            builder.HasKey(p => p.Id);
            builder.OwnsMany(p => p.Brains)
                .HasForeignKey(p => p.TrainingRoomId);
            builder.Metadata.FindNavigation(nameof(TrainingRoom.Brains))
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.OwnsMany(p => p.TrainingSessions)
                .HasForeignKey(p => p.TrainingRoomId);
            builder.Metadata.FindNavigation(nameof(TrainingRoom.TrainingSessions))
                .SetPropertyAccessMode(PropertyAccessMode.Field);
            
            //builder.HasMany(p => p.AuthorizedUsers);
            // NOTE: Ignore users for now...
            builder.Ignore(p => p.AuthorizedUsers);
            builder.Ignore(p => p.Random);
        }
    }
}
