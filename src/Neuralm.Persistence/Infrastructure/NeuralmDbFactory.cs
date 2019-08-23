using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Neuralm.Application.Configurations;
using Neuralm.Application.Interfaces;
using Neuralm.Persistence.Contexts;

namespace Neuralm.Persistence.Infrastructure
{
    /// <summary>
    /// Represents the <see cref="NeuralmDbFactory"/> class; an implementation of the abstract <see cref="DesignTimeDbContextFactoryBase{TContext}"/> class.
    /// </summary>
    public class NeuralmDbFactory : DesignTimeDbContextFactoryBase<NeuralmDbContext>, IFactory<NeuralmDbContext>
    {
        private static readonly string[] Arguments = { "" };

        /// <summary>
        /// Design time constructor IGNORE!
        /// </summary>
        public NeuralmDbFactory() : base(null)
        {

        }

        /// <summary>
        /// Initializes an instance of the <see cref="NeuralmDbFactory"/> class.
        /// </summary>
        /// <param name="neuralmDbConfigurationOptions">The options.</param>
        public NeuralmDbFactory(IOptions<DbConfiguration> neuralmDbConfigurationOptions) : base(neuralmDbConfigurationOptions)
        {

        }

        /// <inheritdoc cref="DesignTimeDbContextFactoryBase{TContext}.CreateNewInstance"/>
        protected override NeuralmDbContext CreateNewInstance(DbContextOptions<NeuralmDbContext> options)
        {
            return new NeuralmDbContext(options);
        }

        /// <inheritdoc cref="IFactory{TResult}.Create"/>
        public NeuralmDbContext Create()
        {
            return base.CreateDbContext(Arguments);
        }
    }
}
