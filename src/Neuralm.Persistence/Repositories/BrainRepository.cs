using Neuralm.Application.Interfaces;
using Neuralm.Domain.Entities.NEAT;
using Neuralm.Persistence.Abstractions;
using Neuralm.Persistence.Contexts;

namespace Neuralm.Persistence.Repositories
{
    /// <summary>
    /// Represents the <see cref="BrainRepository"/> class.
    /// </summary>
    public sealed class BrainRepository : RepositoryBase<Brain, NeuralmDbContext>
    {
        /// <summary>
        /// Initializes an instance of the <see cref="BrainRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="entityValidator">The entity validator.</param>
        public BrainRepository(NeuralmDbContext dbContext, IEntityValidator<Brain> entityValidator) : base(dbContext, entityValidator)
        {

        }
    }
}
