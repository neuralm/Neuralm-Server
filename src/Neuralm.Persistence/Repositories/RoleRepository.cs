using Neuralm.Application.Interfaces;
using Neuralm.Domain.Entities.Authentication;
using Neuralm.Persistence.Abstractions;
using Neuralm.Persistence.Contexts;

namespace Neuralm.Persistence.Repositories
{
    /// <summary>
    /// Represents the <see cref="RoleRepository"/> class.
    /// </summary>
    public class RoleRepository : RepositoryBase<Role, NeuralmDbContext>
    {
        /// <summary>
        /// Initializes an instance of the <see cref="RoleRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="entityValidator">The entity validator.</param>
        public RoleRepository(NeuralmDbContext dbContext, IEntityValidator<Role> entityValidator) : base(dbContext, entityValidator)
        {

        }
    }
}
