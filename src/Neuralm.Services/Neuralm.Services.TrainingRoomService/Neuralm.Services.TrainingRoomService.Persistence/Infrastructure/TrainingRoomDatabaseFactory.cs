using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Neuralm.Services.Common.Configurations;
using Neuralm.Services.Common.Persistence.EFCore.Infrastructure;
using Neuralm.Services.TrainingRoomService.Persistence.Contexts;

namespace Neuralm.Services.TrainingRoomService.Persistence.Infrastructure
{
    /// <summary>
    /// Represents the <see cref="TrainingRoomDatabaseFactory"/> class.
    /// </summary>
    public class TrainingRoomDatabaseFactory : DatabaseFactory<TrainingRoomDbContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TrainingRoomDatabaseFactory"/> class.
        /// </summary>
        public TrainingRoomDatabaseFactory() : base(null, null)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrainingRoomDatabaseFactory"/> class.
        /// </summary>
        /// <param name="dbConfigurationOptions">The options.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public TrainingRoomDatabaseFactory(IOptions<DbConfiguration> dbConfigurationOptions, ILoggerFactory loggerFactory) : base(dbConfigurationOptions, loggerFactory)
        {

        }

        /// <summary>
        /// Creates a new instance of the <see cref="TrainingRoomDbContext"/> class.
        /// </summary>
        /// <param name="dbContextOptions">The options.</param>
        /// <returns>The user database context.</returns>
        protected override TrainingRoomDbContext CreateNewInstance(DbContextOptions<TrainingRoomDbContext> dbContextOptions)
        {
            return new TrainingRoomDbContext(dbContextOptions);
        }
    }
}
