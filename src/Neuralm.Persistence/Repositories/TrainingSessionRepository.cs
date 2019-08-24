using Microsoft.EntityFrameworkCore;
using Neuralm.Application.Interfaces;
using Neuralm.Domain;
using Neuralm.Domain.Entities.NEAT;
using Neuralm.Domain.Exceptions;
using Neuralm.Persistence.Abstractions;
using Neuralm.Persistence.Contexts;
using System;
using System.Threading.Tasks;

namespace Neuralm.Persistence.Repositories
{
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

        /// <inheritdoc cref="RepositoryBase{TEntity,TDbContext}.UpdateAsync(TEntity)"/>
        public override async Task<bool> UpdateAsync(TrainingSession entity)
        {
            bool saveSuccess = false;
            using EntityLoadLock.Releaser loadLock = EntityLoadLock.Shared.Lock();
            DbContext.Update(entity);
            try
            {
                int saveResult = await DbContext.SaveChangesAsync();
                // Force the DbContext to fetch the species anew on next query.
                foreach (Species species in entity.TrainingRoom.Species)
                {
                    DbContext.Entry(species).State = EntityState.Detached;
                }
                saveSuccess = Convert.ToBoolean(saveResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine(new UpdatingEntityFailedException($"The entity of type {typeof(TrainingSession).Name} failed to update.", ex));
            }
            return saveSuccess;
        }
    }
}
