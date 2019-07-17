using Neuralm.Application.Interfaces;
using Neuralm.Domain.Entities.Authentication;
using Neuralm.Persistence.Abstractions;
using Neuralm.Persistence.Contexts;

namespace Neuralm.Persistence.Repositories
{
    /// <summary>
    /// Represents the <see cref="UserRoleRepository"/> class.
    /// </summary>
    public class UserRoleRepository : RepositoryBase<UserRole, NeuralmDbContext>
    {
        /// <summary>
        /// Initializes an instance of the <see cref="UserRoleRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="entityValidator">The entity validator.</param>
        public UserRoleRepository(NeuralmDbContext dbContext, IEntityValidator<UserRole> entityValidator) : base(dbContext, entityValidator)
        {
        }
    }
}
