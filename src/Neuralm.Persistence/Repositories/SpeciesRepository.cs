using Neuralm.Application.Interfaces;
using Neuralm.Domain.Entities.NEAT;
using Neuralm.Persistence.Abstractions;
using Neuralm.Persistence.Contexts;

namespace Neuralm.Persistence.Repositories
{
    /// <summary>
    /// Represents the <see cref="SpeciesRepository"/> class.
    /// </summary>
    public sealed class SpeciesRepository : RepositoryBase<Species, NeuralmDbContext>
    {
        /// <summary>
        /// Initializes an instance of the <see cref="SpeciesRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="entityValidator">The entity validator.</param>
        public SpeciesRepository(NeuralmDbContext dbContext, IEntityValidator<Species> entityValidator) : base(dbContext, entityValidator)
        {

        }
    }
}
