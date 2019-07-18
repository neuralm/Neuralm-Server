using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Neuralm.Application.Interfaces;
using Neuralm.Domain.Entities.NEAT;
using Neuralm.Persistence.Abstractions;
using Neuralm.Persistence.Contexts;

namespace Neuralm.Persistence.Repositories
{
    /// <summary>
    /// Represents the <see cref="TrainingRoomRepository"/> class.
    /// </summary>
    public class TrainingRoomRepository : RepositoryBase<TrainingRoom, NeuralmDbContext>
    {
        /// <summary>
        /// Initializes an instance of the <see cref="TrainingRoomRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="entityValidator">The entity validator.</param>
        public TrainingRoomRepository(NeuralmDbContext dbContext, IEntityValidator<TrainingRoom> entityValidator) : base(dbContext, entityValidator)
        {

        }

        /// <inheritdoc cref="RepositoryBase{TEntity,TDbContext}.CreateAsync"/>
        public override Task<bool> CreateAsync(TrainingRoom entity)
        {
            DbContext.Entry(entity.Owner).State = EntityState.Unchanged;
            return base.CreateAsync(entity);
        }
    }
}
