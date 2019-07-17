using Neuralm.Application.Interfaces;
using Neuralm.Domain.Entities.Authentication;
using Neuralm.Persistence.Abstractions;
using Neuralm.Persistence.Contexts;

namespace Neuralm.Persistence.Repositories
{
    /// <summary>
    /// Represents the <see cref="PermissionRepository"/> class.
    /// </summary>
    public class PermissionRepository : RepositoryBase<Permission, NeuralmDbContext>
    {
        /// <summary>
        /// Initializes an instance of the <see cref="PermissionRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="entityValidator">The entity validator.</param>
        public PermissionRepository(NeuralmDbContext dbContext, IEntityValidator<Permission> entityValidator) : base(dbContext, entityValidator)
        {

        }
    }
}
