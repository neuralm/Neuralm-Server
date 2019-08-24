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
    /// <summary>
    /// Represents the <see cref="TrainingRoomRepository"/> class.
    /// </summary>
    public sealed class TrainingRoomRepository : RepositoryBase<TrainingRoom, NeuralmDbContext>
    {
        /// <summary>
        /// Initializes an instance of the <see cref="TrainingRoomRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="entityValidator">The entity validator.</param>
        public TrainingRoomRepository(NeuralmDbContext dbContext, IEntityValidator<TrainingRoom> entityValidator) : base(dbContext, entityValidator)
        {

        }

        /// <inheritdoc cref="RepositoryBase{TEntity,TDbContext}.CreateAsync(TEntity)"/>
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
    }
}
