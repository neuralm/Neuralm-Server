using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neuralm.Services.TrainingRoomService.Domain;

namespace Neuralm.Services.TrainingRoomService.Persistence.Configurations
{
    /// <summary>
    /// Represents the <see cref="TrainerConfiguration"/> class used to configure the relations and columns in the <see cref="DbSet{TEntity}"/> for <see cref="Trainer"/> in the DbContext.
    /// </summary>
    public class TrainerConfiguration : IEntityTypeConfiguration<Trainer>
    {
        /// <inheritdoc cref="IEntityTypeConfiguration{TEntity}.Configure"/>
        public void Configure(EntityTypeBuilder<Trainer> builder)
        {
            builder.Ignore(p => p.Id);
            builder.HasKey(p => new { p.TrainingRoomId, p.UserId });
            builder.Ignore(p => p.User);
            //builder
            //    .HasOne(p => p.User)
            //    .WithMany()
            //    .HasForeignKey(p => p.UserId)
            //    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
