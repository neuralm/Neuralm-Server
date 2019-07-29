using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Neuralm.Application.Interfaces;
using Neuralm.Domain;
using Neuralm.Domain.Entities.NEAT;
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

        /// <inheritdoc cref="RepositoryBase{TEntity,TDbContext}.FindSingleOrDefaultAsync"/>
        public override async Task<TrainingSession> FindSingleOrDefaultAsync(Expression<Func<TrainingSession, bool>> predicate)
        {
            using EntityLoadLock.Releaser loadLock = EntityLoadLock.Shared.Lock();
            return await TrainingSessionSetWithInclude().Where(predicate).SingleOrDefaultAsync();
        }

        private IQueryable<TrainingSession> TrainingSessionSetWithInclude()
        {
            return DbContext.Set<TrainingSession>()
                .Include("TrainingRoom.TrainingRoomSettings")
                .Include("TrainingRoom.AuthorizedTrainers")
                .Include("TrainingRoom.Organisms.Brain")
                .Include("TrainingRoom.Species.LastGenerationOrganisms.Brain")
                .Include("TrainingRoom.Species.LastGenerationOrganisms.TrainingRoom");
        }
    }
}
