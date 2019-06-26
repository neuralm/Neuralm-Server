using Neuralm.Application.Interfaces;
using Neuralm.Domain.Entities.Authentication;
using Neuralm.Persistence.Abstractions;
using Neuralm.Persistence.Contexts;

namespace Neuralm.Persistence.Repositories
{
    public class CredentialTypeRepository : RepositoryBase<CredentialType, NeuralmDbContext>
    {
        public CredentialTypeRepository(NeuralmDbContext dbContext, IEntityValidator<CredentialType> entityValidator) : base(dbContext, entityValidator)
        {
        }
    }
}
