using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neuralm.Domain.Entities.NEAT;

namespace Neuralm.Persistence.Configurations
{
    internal class ConnectionGeneConfiguration : IEntityTypeConfiguration<ConnectionGene>
    {
        public void Configure(EntityTypeBuilder<ConnectionGene> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Ignore(p => p.InNode);
            builder.Ignore(p => p.OutNode);
        }
    }
}
