using Neuralm.Application.Interfaces;
using Neuralm.Persistence.Abstractions;
using Neuralm.Persistence.Contexts;

namespace Neuralm.Persistence.Repositories
{
    /// <summary>
    /// Represents the <see cref="NeuralmDbContextRepository{TEntity}"/> class.
    /// The default implementation of the <see cref="RepositoryBase{TEntity,TDbContext}"/> class.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public sealed class NeuralmDbContextRepository<TEntity> : RepositoryBase<TEntity, NeuralmDbContext> where TEntity : class
    {
        /// <summary>
        /// Initializes an instance of the <see cref="NeuralmDbContextRepository{TEntity}"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="entityValidator">The entity validator.</param>
        public NeuralmDbContextRepository(NeuralmDbContext dbContext, IEntityValidator<TEntity> entityValidator) : base(dbContext, entityValidator)
        {

        }
    }
}
