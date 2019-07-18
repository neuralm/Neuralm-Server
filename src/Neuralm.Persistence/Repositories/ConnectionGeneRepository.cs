using Neuralm.Application.Interfaces;
using Neuralm.Domain.Entities.NEAT;
using Neuralm.Persistence.Abstractions;
using Neuralm.Persistence.Contexts;

namespace Neuralm.Persistence.Repositories
{
    /// <summary>
    /// Represents the <see cref="ConnectionGeneRepository"/> class.
    /// </summary>
    public sealed class ConnectionGeneRepository : RepositoryBase<ConnectionGene, NeuralmDbContext>
    {
        /// <summary>
        /// Initializes an instance of the <see cref="ConnectionGeneRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="entityValidator">The entity validator.</param>
        public ConnectionGeneRepository(NeuralmDbContext dbContext, IEntityValidator<ConnectionGene> entityValidator) : base(dbContext, entityValidator)
        {

        }
    }
}
