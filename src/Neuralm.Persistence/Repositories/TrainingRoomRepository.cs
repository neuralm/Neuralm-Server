using Neuralm.Application.Interfaces;
using Neuralm.Domain.Entities.NEAT;
using Neuralm.Persistence.Abstractions;
using Neuralm.Persistence.Contexts;

namespace Neuralm.Persistence.Repositories
{
    public class TrainingRoomRepository : RepositoryBase<TrainingRoom, NeuralmDbContext>
    {
        public TrainingRoomRepository(NeuralmDbContext dbContext, IEntityValidator<TrainingRoom> entityValidator) : base(dbContext, entityValidator)
        {

        }
    }
}
