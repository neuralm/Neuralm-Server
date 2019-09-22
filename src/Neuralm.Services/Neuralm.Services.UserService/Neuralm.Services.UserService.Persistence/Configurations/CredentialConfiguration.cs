using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neuralm.Services.UserService.Domain.Authentication;

namespace Neuralm.Services.UserService.Persistence.Configurations
{
    /// <summary>
    /// Represents the <see cref="CredentialConfiguration"/> class used to configure the relations and columns in the <see cref="DbSet{TEntity}"/> for <see cref="Credential"/> in the DbContext.
    /// </summary>
    public class CredentialConfiguration : IEntityTypeConfiguration<Credential>
    {
        /// <inheritdoc cref="IEntityTypeConfiguration{TEntity}.Configure"/>
        public void Configure(EntityTypeBuilder<Credential> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.Identifier).IsRequired().HasMaxLength(64);
            builder.Property(e => e.Secret).HasMaxLength(1024);
        }
    }
}
