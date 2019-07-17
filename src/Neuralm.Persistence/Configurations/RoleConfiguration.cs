using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neuralm.Domain.Entities.Authentication;

namespace Neuralm.Persistence.Configurations
{
    /// <summary>
    /// The RoleConfiguration class; used to configure the relations and columns in the <see cref="DbSet{TEntity}"/> for <see cref="Role"/> in the DbContext.
    /// </summary>
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        /// <inheritdoc cref="IEntityTypeConfiguration{TEntity}.Configure"/>
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.Code).IsRequired().HasMaxLength(32);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(64);
        }
    }
}
