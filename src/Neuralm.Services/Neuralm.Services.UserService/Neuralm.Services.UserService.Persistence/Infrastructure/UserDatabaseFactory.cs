using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Neuralm.Services.Common.Configurations;
using Neuralm.Services.Common.Persistence.EFCore.Infrastructure;
using Neuralm.Services.UserService.Persistence.Contexts;

namespace Neuralm.Services.UserService.Persistence.Infrastructure
{
    /// <summary>
    /// Represents the <see cref="UserDatabaseFactory"/> class.
    /// </summary>
    public class UserDatabaseFactory : DatabaseFactory<UserDbContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserDatabaseFactory"/> class.
        /// </summary>
        public UserDatabaseFactory() : base(null, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDatabaseFactory"/> class.
        /// </summary>
        /// <param name="dbConfigurationOptions">The options.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public UserDatabaseFactory(IOptions<DbConfiguration> dbConfigurationOptions, ILoggerFactory loggerFactory) : base(dbConfigurationOptions, loggerFactory)
        {

        }

        /// <summary>
        /// Creates a new instance of the <see cref="UserDbContext"/> class.
        /// </summary>
        /// <param name="dbContextOptions">The options.</param>
        /// <returns>The user database context.</returns>
        protected override UserDbContext CreateNewInstance(DbContextOptions<UserDbContext> dbContextOptions)
        {
            return new UserDbContext(dbContextOptions);
        }
    }
}
