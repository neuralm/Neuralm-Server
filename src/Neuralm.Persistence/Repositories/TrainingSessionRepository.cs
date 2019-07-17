using Neuralm.Application.Interfaces;
using Neuralm.Domain.Entities.NEAT;
using Neuralm.Persistence.Abstractions;
using Neuralm.Persistence.Contexts;

namespace Neuralm.Persistence.Repositories
{
    /// <summary>
    /// Represents the <see cref="TrainingSessionRepository"/> class.
    /// </summary>
    public sealed class TrainingSessionRepository : RepositoryBase<TrainingSession, NeuralmDbContext>
    {
        /// <summary>
        /// Initializes an instance of the <see cref="TrainingSessionRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="entityValidator">The entity validator.</param>
        public TrainingSessionRepository(NeuralmDbContext dbContext, IEntityValidator<TrainingSession> entityValidator) : base(dbContext, entityValidator)
        {

        }
    }
}
