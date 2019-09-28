using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neuralm.Services.TrainingRoomService.Domain;

namespace Neuralm.Services.TrainingRoomService.Persistence.Configurations
{
    /// <summary>
    /// Represents the <see cref="OrganismConfiguration"/> class used to configure the relations and columns in the <see cref="DbSet{TEntity}"/> for <see cref="Organism"/> in the DbContext.
    /// </summary>
    public class OrganismConfiguration : IEntityTypeConfiguration<Organism>
    {
        /// <inheritdoc cref="IEntityTypeConfiguration{TEntity}.Configure"/>
        public void Configure(EntityTypeBuilder<Organism> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder
                .HasMany(p => p.ConnectionGenes)
                .WithOne()
                .HasForeignKey(p => p.OrganismId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
