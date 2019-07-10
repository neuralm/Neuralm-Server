using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neuralm.Domain.Entities.NEAT;
using Newtonsoft.Json;

namespace Neuralm.Persistence.Configurations
{
    internal class BrainConfiguration : IEntityTypeConfiguration<Brain>
    {
        public void Configure(EntityTypeBuilder<Brain> builder)
        {
            builder.HasKey(p => p.Id);
            builder
                .Property(e => e.Genes)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => v == null
                        ? new Dictionary<uint, ConnectionGene>()
                        : JsonConvert.DeserializeObject<Dictionary<uint, ConnectionGene>>(v)
                );
        }
    }
}
