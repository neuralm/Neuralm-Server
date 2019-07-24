using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Neuralm.Application.Interfaces;
using Neuralm.Domain;
using Neuralm.Domain.Entities.NEAT;
using Neuralm.Domain.Exceptions;
using Neuralm.Persistence.Abstractions;
using Neuralm.Persistence.Contexts;

namespace Neuralm.Persistence.Repositories
{
    /// <summary>
    /// Represents the <see cref="TrainingSessionRepository"/> class.
    /// </summary>
    public sealed class TrainingSessionRepository : RepositoryBase<TrainingSession, NeuralmDbContext>
    {
        /// <summary>
        /// Initializes an instance of the <see cref="TrainingSessionRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="entityValidator">The entity validator.</param>
        public TrainingSessionRepository(NeuralmDbContext dbContext, IEntityValidator<TrainingSession> entityValidator) : base(dbContext, entityValidator)
        {

        }

        /// <inheritdoc cref="RepositoryBase{TEntity,TDbContext}.FindSingleByExpressionAsync"/>
        public override async Task<TrainingSession> FindSingleByExpressionAsync(Expression<Func<TrainingSession, bool>> predicate)
        {
            using EntityLoadLock.Releaser loadLock = EntityLoadLock.Shared.Lock();
            TrainingSession entity = await TrainingSessionSetWithInclude().Where(predicate).SingleOrDefaultAsync();
            if (entity == default)
                Console.WriteLine(new EntityNotFoundException($"The entity of type {typeof(TrainingSession).Name} could not be found by the predicate."));
            return entity;
        }

        private IQueryable<TrainingSession> TrainingSessionSetWithInclude()
        {
            return DbContext.Set<TrainingSession>()
                .Include("TrainingRoom.TrainingRoomSettings")
                .Include("TrainingRoom.Organisms.Brain")
                .Include("TrainingRoom.Species.LastGenerationOrganisms.Brain")
                .Include("TrainingRoom.Species.LastGenerationOrganisms.TrainingRoom");
            ;
        }
    }
}
