using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neuralm.Domain.Entities.Authentication;

namespace Neuralm.Persistence.Configurations
{
    public class CredentialConfiguration : IEntityTypeConfiguration<Credential>
    {
        public void Configure(EntityTypeBuilder<Credential> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.Identifier).IsRequired().HasMaxLength(64);
            builder.Property(e => e.Secret).HasMaxLength(1024);
        }
    }
}
