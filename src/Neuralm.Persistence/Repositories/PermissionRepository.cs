using Neuralm.Application.Interfaces;
using Neuralm.Domain.Entities.Authentication;
using Neuralm.Persistence.Abstractions;
using Neuralm.Persistence.Contexts;

namespace Neuralm.Persistence.Repositories
{
    public class PermissionRepository : RepositoryBase<Permission, NeuralmDbContext>
    {
        public PermissionRepository(NeuralmDbContext dbContext, IEntityValidator<Permission> entityValidator) : base(dbContext, entityValidator)
        {
        }
    }
}
