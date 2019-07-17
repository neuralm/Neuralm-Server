using Neuralm.Application.Interfaces;
using Neuralm.Domain.Entities.NEAT;
using Neuralm.Persistence.Abstractions;
using Neuralm.Persistence.Contexts;

namespace Neuralm.Persistence.Repositories
{
    /// <summary>
    /// Represents the <see cref="TrainingRoomSettingsRepository"/> class.
    /// </summary>
    public class TrainingRoomSettingsRepository : RepositoryBase<TrainingRoomSettings, NeuralmDbContext>
    {
        /// <summary>
        /// Initializes an instance of the <see cref="TrainingRoomSettingsRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="entityValidator">The entity validator.</param>
        public TrainingRoomSettingsRepository(NeuralmDbContext dbContext, IEntityValidator<TrainingRoomSettings> entityValidator) : base(dbContext, entityValidator)
        {

        }
    }
}
