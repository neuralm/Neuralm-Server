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
            builder.HasMany(tr => tr.TrainingSessions)
                .WithOne(room => room.TrainingRoom)
                .HasForeignKey(room => room.TrainingRoomId);
            builder.HasMany(p => p.AuthorizedUsers);
            builder.Ignore(p => p.Random);
        }
    }
}
