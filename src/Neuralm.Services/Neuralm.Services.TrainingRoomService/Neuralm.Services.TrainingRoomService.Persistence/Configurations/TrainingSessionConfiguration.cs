using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neuralm.Services.TrainingRoomService.Domain;

namespace Neuralm.Services.TrainingRoomService.Persistence.Configurations
{
    /// <summary>
    /// Represents the <see cref="TrainingSessionConfiguration"/> class used to configure the relations and columns in the <see cref="DbSet{TEntity}"/> for <see cref="TrainingSession"/> in the DbContext.
    /// </summary>
    public class TrainingSessionConfiguration : IEntityTypeConfiguration<TrainingSession>
    {
        /// <inheritdoc cref="IEntityTypeConfiguration{TEntity}.Configure"/>
        public void Configure(EntityTypeBuilder<TrainingSession> builder)
        {
            builder.HasKey(p => p.Id);
            builder
                .HasOne(p => p.TrainingRoom)
                .WithMany(p => p.TrainingSessions)
                .HasForeignKey(p => p.TrainingRoomId);

            builder
                .OwnsMany(p => p.LeasedOrganisms)
                .OnDelete(DeleteBehavior.Cascade)
                .HasKey(p => p.Id);
        }
    }
}
