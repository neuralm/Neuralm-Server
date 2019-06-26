using Neuralm.Application.Interfaces;
using Neuralm.Domain.Entities.Authentication;
using Neuralm.Persistence.Abstractions;
using Neuralm.Persistence.Contexts;

namespace Neuralm.Persistence.Repositories
{
    public class UserRoleRepository : RepositoryBase<UserRole, NeuralmDbContext>
    {
        public UserRoleRepository(NeuralmDbContext dbContext, IEntityValidator<UserRole> entityValidator) : base(dbContext, entityValidator)
        {
        }
    }
}
