using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neuralm.Services.TrainingRoomService.Domain;

namespace Neuralm.Services.TrainingRoomService.Persistence.Configurations
{
    /// <summary>
    /// Represents the <see cref="TrainingRoomConfiguration"/> class used to configure the relations and columns in the <see cref="DbSet{TEntity}"/> for <see cref="TrainingRoom"/> in the DbContext.
    /// </summary>
    public class TrainingRoomConfiguration : IEntityTypeConfiguration<TrainingRoom>
    {
        /// <inheritdoc cref="IEntityTypeConfiguration{TEntity}.Configure"/>
        public void Configure(EntityTypeBuilder<TrainingRoom> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Ignore(p => p.Owner);
            builder
                .HasMany(p => p.Species)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .OwnsOne(p => p.TrainingRoomSettings, (trainingRoomSettingsBuilder) =>
                {
                    trainingRoomSettingsBuilder.HasKey(p => p.Id);
                    trainingRoomSettingsBuilder.Property(p => p.Id).ValueGeneratedOnAdd();
                    trainingRoomSettingsBuilder.ToTable("TrainingRoomSettings");
                    trainingRoomSettingsBuilder.Ignore(p => p.Random);
                });

            builder
                .OwnsMany(p => p.AuthorizedTrainers, (trainerBuilder) =>
                {
                    trainerBuilder
                        .WithOwner(p => p.TrainingRoom)
                        .HasForeignKey(p => p.TrainingRoomId);
                    trainerBuilder.Ignore(p => p.Id);
                    trainerBuilder.HasKey(p => new { p.TrainingRoomId, p.UserId });
                    trainerBuilder.Ignore(p => p.User);
                });
        }
    }
}
