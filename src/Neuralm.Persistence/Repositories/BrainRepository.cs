using Neuralm.Application.Interfaces;
using Neuralm.Domain.Entities.NEAT;
using Neuralm.Persistence.Abstractions;
using Neuralm.Persistence.Contexts;

namespace Neuralm.Persistence.Repositories
{
    public sealed class BrainRepository : RepositoryBase<Brain, NeuralmDbContext>
    {
        public BrainRepository(NeuralmDbContext dbContext, IEntityValidator<Brain> entityValidator) : base(dbContext, entityValidator)
        {

        }
    }
}
