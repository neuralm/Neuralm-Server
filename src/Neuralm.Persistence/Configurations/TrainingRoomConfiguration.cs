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
            builder.HasMany(p => p.TrainingSessions);
            builder.HasMany(p => p.AuthorizedUsers);
        }
    }
}
