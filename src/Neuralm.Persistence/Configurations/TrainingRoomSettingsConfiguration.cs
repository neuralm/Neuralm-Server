using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neuralm.Domain.Entities.NEAT;

namespace Neuralm.Persistence.Configurations
{
    internal class TrainingRoomSettingsConfiguration : IEntityTypeConfiguration<TrainingRoomSettings>
    {
        public void Configure(EntityTypeBuilder<TrainingRoomSettings> builder)
        {
            builder.HasKey(p => p.Id);
        }
    }
}
