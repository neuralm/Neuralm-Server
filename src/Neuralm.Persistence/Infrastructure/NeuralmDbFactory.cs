using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Neuralm.Application.Configurations;
using Neuralm.Application.Interfaces;
using Neuralm.Persistence.Contexts;

namespace Neuralm.Persistence.Infrastructure
{
    public class NeuralmDbFactory : DesignTimeDbContextFactoryBase<NeuralmDbContext>, IFactory<NeuralmDbContext>
    {
        private static readonly string[] Arguments = { "" };

        public NeuralmDbFactory() : base(null)
        {

        }
        public NeuralmDbFactory(IOptions<DbConfiguration> neuralmDbConfigurationOptions) : base(neuralmDbConfigurationOptions)
        {

        }

        protected override NeuralmDbContext CreateNewInstance(DbContextOptions<NeuralmDbContext> options)
        {
            return new NeuralmDbContext(options);
        }
        public NeuralmDbContext Create()
        {
            return base.CreateDbContext(Arguments);
        }
    }
}
