using Neuralm.Application.Interfaces;
using Neuralm.Domain.Entities.Authentication;
using Neuralm.Persistence.Abstractions;
using Neuralm.Persistence.Contexts;

namespace Neuralm.Persistence.Repositories
{
    public class CredentialRepository : RepositoryBase<Credential, NeuralmDbContext>
    {
        public CredentialRepository(NeuralmDbContext dbContext, IEntityValidator<Credential> entityValidator) : base(dbContext, entityValidator)
        {
        }
    }
}
