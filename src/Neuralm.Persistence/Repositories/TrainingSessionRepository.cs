using Neuralm.Application.Interfaces;
using Neuralm.Domain.Entities.NEAT;
using Neuralm.Persistence.Abstractions;
using Neuralm.Persistence.Contexts;

namespace Neuralm.Persistence.Repositories
{
    public sealed class TrainingSessionRepository : RepositoryBase<TrainingSession, NeuralmDbContext>
    {
        public TrainingSessionRepository(NeuralmDbContext dbContext, IEntityValidator<TrainingSession> entityValidator) : base(dbContext, entityValidator)
        {

        }
    }
}
