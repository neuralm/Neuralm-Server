using Neuralm.Application.Interfaces;
using Neuralm.Domain.Entities.NEAT;
using Neuralm.Persistence.Abstractions;
using Neuralm.Persistence.Contexts;

namespace Neuralm.Persistence.Repositories
{
    public class TrainingRoomSettingsRepository : RepositoryBase<TrainingRoomSettings, NeuralmDbContext>
    {
        public TrainingRoomSettingsRepository(NeuralmDbContext dbContext, IEntityValidator<TrainingRoomSettings> entityValidator) : base(dbContext, entityValidator)
        {

        }
    }
}
