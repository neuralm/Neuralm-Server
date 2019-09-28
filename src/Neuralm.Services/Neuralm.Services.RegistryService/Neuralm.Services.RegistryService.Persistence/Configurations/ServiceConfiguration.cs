using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neuralm.Services.RegistryService.Domain;

namespace Neuralm.Services.RegistryService.Persistence.Configurations
{
    /// <summary>
    /// Represents the <see cref="ServiceConfiguration"/> class used to configure the relations and columns
    /// in the <see cref="DbSet{TEntity}"/> for <see cref="Service"/> in the DbContext.
    /// </summary>
    public class ServiceConfiguration : IEntityTypeConfiguration<Service>
    {
        /// <inheritdoc cref="IEntityTypeConfiguration{TEntity}.Configure"/>
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
        }
    }
}
