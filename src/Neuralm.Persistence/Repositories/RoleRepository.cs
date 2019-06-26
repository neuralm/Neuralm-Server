using Neuralm.Application.Interfaces;
using Neuralm.Domain.Entities.Authentication;
using Neuralm.Persistence.Abstractions;
using Neuralm.Persistence.Contexts;

namespace Neuralm.Persistence.Repositories
{
    public class RoleRepository : RepositoryBase<Role, NeuralmDbContext>
    {
        public RoleRepository(NeuralmDbContext dbContext, IEntityValidator<Role> entityValidator) : base(dbContext, entityValidator)
        {
        }
    }
}
