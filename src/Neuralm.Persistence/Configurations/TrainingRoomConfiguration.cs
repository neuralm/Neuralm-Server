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
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder
                .HasMany(p => p.Species)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .OwnsOne(p => p.TrainingRoomSettings)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .OwnsMany(p => p.AuthorizedTrainers)
                .HasForeignKey(p => p.TrainingRoomId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
