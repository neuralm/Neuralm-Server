using Microsoft.EntityFrameworkCore;
using Neuralm.Services.Common.Persistence.EFCore.Extensions;

namespace Neuralm.Services.TrainingRoomService.Persistence.Contexts
{
    /// <summary>
    /// Represents the <see cref="TrainingRoomDbContext"/> class.
    /// </summary>
    public class TrainingRoomDbContext : DbContext
    {
        /// <summary>
        /// Initializes an instance of the <see cref="TrainingRoomDbContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public TrainingRoomDbContext(DbContextOptions<TrainingRoomDbContext> options) : base(options)
        {

        }

        /// <summary>
        /// Applies all entity configurations.
        /// </summary>
        /// <param name="modelBuilder">the model builder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyAllConfigurations<TrainingRoomDbContext>();
            base.OnModelCreating(modelBuilder);
        }
    }
}
