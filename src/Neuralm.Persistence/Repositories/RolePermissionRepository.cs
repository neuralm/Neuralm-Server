using Neuralm.Application.Interfaces;
using Neuralm.Domain.Entities.Authentication;
using Neuralm.Persistence.Abstractions;
using Neuralm.Persistence.Contexts;

namespace Neuralm.Persistence.Repositories
{
    /// <summary>
    /// Represents the <see cref="RolePermissionRepository"/> class.
    /// </summary>
    public class RolePermissionRepository : RepositoryBase<RolePermission, NeuralmDbContext>
    {
        /// <summary>
        /// Initializes an instance of the <see cref="RolePermissionRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="entityValidator">The entity validator.</param>
        public RolePermissionRepository(NeuralmDbContext dbContext, IEntityValidator<RolePermission> entityValidator) : base(dbContext, entityValidator)
        {

        }
    }
}
