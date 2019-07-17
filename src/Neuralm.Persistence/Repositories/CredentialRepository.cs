using Neuralm.Application.Interfaces;
using Neuralm.Domain.Entities.Authentication;
using Neuralm.Persistence.Abstractions;
using Neuralm.Persistence.Contexts;

namespace Neuralm.Persistence.Repositories
{
    /// <summary>
    /// Represents the <see cref="CredentialRepository"/> class.
    /// </summary>
    public class CredentialRepository : RepositoryBase<Credential, NeuralmDbContext>
    {
        /// <summary>
        /// Initializes an instance of the <see cref="CredentialRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="entityValidator">The entity validator.</param>
        public CredentialRepository(NeuralmDbContext dbContext, IEntityValidator<Credential> entityValidator) : base(dbContext, entityValidator)
        {

        }
    }
}
