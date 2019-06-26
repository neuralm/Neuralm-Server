using Neuralm.Application.Interfaces;
using Neuralm.Domain.Entities.Authentication;
using Neuralm.Persistence.Abstractions;
using Neuralm.Persistence.Contexts;

namespace Neuralm.Persistence.Repositories
{
    public class RolePermissionRepository : RepositoryBase<RolePermission, NeuralmDbContext>
    {
        public RolePermissionRepository(NeuralmDbContext dbContext, IEntityValidator<RolePermission> entityValidator) : base(dbContext, entityValidator)
        {
        }
    }
}
