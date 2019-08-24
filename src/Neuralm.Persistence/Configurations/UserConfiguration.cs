using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neuralm.Domain.Entities;

namespace Neuralm.Persistence.Configurations
{
    /// <summary>
    /// Represents the <see cref="UserConfiguration"/> class used to configure the relations and columns in the <see cref="DbSet{TEntity}"/> for <see cref="User"/> in the DbContext.
    /// </summary>
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        /// <inheritdoc cref="IEntityTypeConfiguration{TEntity}.Configure"/>
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(user => user.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.Username).IsRequired().HasMaxLength(64);
            builder.Property(p => p.TimestampCreated).ValueGeneratedOnAdd();
        }
    }
}
