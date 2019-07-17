using Neuralm.Application.Interfaces;
using Neuralm.Domain.Entities;
using Neuralm.Persistence.Abstractions;
using Neuralm.Persistence.Contexts;

namespace Neuralm.Persistence.Repositories
{
    /// <summary>
    /// Represents the <see cref="UserRepository"/> class.
    /// </summary>
    public class UserRepository : RepositoryBase<User, NeuralmDbContext>
    {
        /// <summary>
        /// Initializes an instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="entityValidator">The entity validator.</param>
        public UserRepository(NeuralmDbContext dbContext, IEntityValidator<User> entityValidator) : base(dbContext, entityValidator)
        {

        }
    }
}
