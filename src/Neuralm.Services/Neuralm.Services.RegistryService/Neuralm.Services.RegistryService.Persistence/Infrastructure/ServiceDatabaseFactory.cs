using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Neuralm.Services.Common.Configurations;
using Neuralm.Services.Common.Persistence.EFCore.Infrastructure;
using Neuralm.Services.RegistryService.Persistence.Contexts;

namespace Neuralm.Services.RegistryService.Persistence.Infrastructure
{
    /// <summary>
    /// Represents the <see cref="ServiceDatabaseFactory"/> class.
    /// </summary>
    public class ServiceDatabaseFactory : DatabaseFactory<ServiceDbContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceDatabaseFactory"/> class.
        /// </summary>
        public ServiceDatabaseFactory() : base(null, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceDatabaseFactory"/> class.
        /// </summary>
        /// <param name="dbConfigurationOptions">The options.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public ServiceDatabaseFactory(IOptions<DbConfiguration> dbConfigurationOptions, ILoggerFactory loggerFactory) : base(dbConfigurationOptions, loggerFactory)
        {

        }

        /// <summary>
        /// Creates a new instance of the <see cref="ServiceDbContext"/> class.
        /// </summary>
        /// <param name="dbContextOptions">The options.</param>
        /// <returns>The user database context.</returns>
        protected override ServiceDbContext CreateNewInstance(DbContextOptions<ServiceDbContext> dbContextOptions)
        {
            return new ServiceDbContext(dbContextOptions);
        }
    }
}
