using System;
using System.Collections.Generic;
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
        public override async Task<bool> CreateAsync(TrainingRoom entity)
        {
            bool saveSuccess = false;
            using EntityLoadLock.Releaser loadLock = EntityLoadLock.Shared.Lock();
            try
            {
                DbContext.Entry(entity.Owner).State = EntityState.Unchanged;
                EntityValidator.Validate(entity);
                DbContext.Set<TrainingRoom>().Add(entity);
                int saveResult = await DbContext.SaveChangesAsync();
                saveSuccess = Convert.ToBoolean(saveResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine(new CreatingEntityFailedException($"The entity of type {typeof(TrainingRoom).Name} could not be created.", ex));
            }

            return saveSuccess;
        }

        /// <inheritdoc cref="RepositoryBase{TEntity,TDbContext}.FindSingleByExpressionAsync"/>
        public override async Task<TrainingRoom> FindSingleByExpressionAsync(Expression<Func<TrainingRoom, bool>> predicate)
        {
            using EntityLoadLock.Releaser loadLock = EntityLoadLock.Shared.Lock();
            TrainingRoom entity = await TrainingRoomSetWithInclude().Where(predicate).SingleOrDefaultAsync();
            if (entity == default)
                Console.WriteLine(new EntityNotFoundException($"The entity of type {typeof(TrainingRoom).Name} could not be found by the predicate."));
            return entity;
        }

        /// <inheritdoc cref="RepositoryBase{TEntity,TDbContext}.FindManyByExpressionAsync"/>
        public override async Task<IEnumerable<TrainingRoom>> FindManyByExpressionAsync(Expression<Func<TrainingRoom, bool>> predicate)
        {
            using EntityLoadLock.Releaser loadLock = EntityLoadLock.Shared.Lock();
            return await TrainingRoomSetWithInclude().Where(predicate).ToListAsync();
        }

        /// <inheritdoc cref="RepositoryBase{TEntity,TDbContext}.GetAllAsync"/>
        public override async Task<IEnumerable<TrainingRoom>> GetAllAsync()
        {
            using EntityLoadLock.Releaser loadLock = EntityLoadLock.Shared.Lock();
            return await TrainingRoomSetWithInclude().ToListAsync();
        }

        private IQueryable<TrainingRoom> TrainingRoomSetWithInclude()
        {
            return DbContext.Set<TrainingRoom>()
                .Include("TrainingRoomSettings")
                .Include("Brains.ConnectionGenes")
                .Include("Organisms.Brain")
                .Include("Owner.Credentials.CredentialType")
                .Include("Species.LastGenerationOrganisms.Brain")
                .Include("Species.LastGenerationOrganisms.TrainingRoom")
                .Include("AuthorizedTrainers")
                .Include("TrainingSessions");
        }
    }
}
