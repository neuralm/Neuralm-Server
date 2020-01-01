using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Neuralm.Services.Common.Exceptions;
using Neuralm.Services.Common.Persistence;
using Neuralm.Services.Common.Persistence.EFCore;
using Neuralm.Services.Common.Persistence.EFCore.Abstractions;
using Neuralm.Services.TrainingRoomService.Domain;
using Neuralm.Services.TrainingRoomService.Persistence.Contexts;
using System;
using System.Linq;
using System.Threading.Tasks;
using Neuralm.Services.TrainingRoomService.Application.Interfaces;

namespace Neuralm.Services.TrainingRoomService.Persistence.Repositories
{
    /// <summary>
    /// Represents the <see cref="TrainingSessionRepository"/> class.
    /// </summary>
    public sealed class TrainingSessionRepository : RepositoryBase<TrainingSession, TrainingRoomDbContext>, ITrainingSessionRepository
    {
        /// <summary>
        /// Initializes an instance of the <see cref="TrainingSessionRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="entityValidator">The entity validator.</param>
        public TrainingSessionRepository(TrainingRoomDbContext dbContext, IEntityValidator<TrainingSession> entityValidator) : base(dbContext, entityValidator)
        {

        }

        public override async Task<(bool success, Guid id)> CreateAsync(TrainingSession entity)
        {
            bool saveSuccess = false;
            using EntityLoadLock.Releaser loadLock = EntityLoadLock.Shared.Lock();
            try
            {
                EntityValidator.Validate(entity);
                DbContext.Entry(entity.TrainingRoom).State = EntityState.Unchanged;
                DbContext.Set<TrainingSession>().Add(entity);
                int saveResult = await DbContext.SaveChangesAsync();
                // Force the DbContext to fetch the species anew on next query.
                foreach (TrainingSession session in entity.TrainingRoom.TrainingSessions)
                {
                    DbContext.Entry(session).State = EntityState.Detached;
                }
                saveSuccess = Convert.ToBoolean(saveResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine(new CreatingEntityFailedException($"The entity of type {typeof(TrainingSession).Name} could not be created.", ex));
            }
            return (success: saveSuccess, id: entity.Id);
        }

        /// <inheritdoc cref="ITrainingSessionRepository.InsertFirstGenerationAsync(TrainingRoom)"/>
        public async Task InsertFirstGenerationAsync(TrainingSession trainingSession)
        {
            using EntityLoadLock.Releaser loadLock = EntityLoadLock.Shared.Lock();
            try
            {
                foreach (Species species in trainingSession.TrainingRoom.Species.Where(species => DbContext.Entry(species).State != EntityState.Unchanged))
                {
                    DbContext.Entry(species).State = EntityState.Added;
                    foreach (Organism organism in species.Organisms)
                    {
                        DbContext.Entry(organism).State = EntityState.Added;
                        foreach (OrganismInputNode inputNode in organism.Inputs)
                        {
                            DbContext.Entry(inputNode).State = EntityState.Added;
                            DbContext.Entry(inputNode.InputNode).State = EntityState.Added;
                        }
                        foreach (OrganismOutputNode outputNode in organism.Outputs)
                        {
                            DbContext.Entry(outputNode).State = EntityState.Added;
                            DbContext.Entry(outputNode.OutputNode).State = EntityState.Added;
                        }
                        foreach (ConnectionGene connectionGene in organism.ConnectionGenes)
                        {
                            DbContext.Entry(connectionGene).State = EntityState.Added;
                        }
                    }
                }

                foreach (LeasedOrganism leasedOrganism in trainingSession.LeasedOrganisms)
                {
                    DbContext.Entry(leasedOrganism).State = EntityState.Added;
                }

                await DbContext.SaveChangesAsync();

                DbContext.Update(trainingSession.TrainingRoom);
                
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(new CreatingEntityFailedException($"The entity of type {typeof(TrainingRoom).Name} could not be created.", ex));
            }
        }

        /// <inheritdoc cref="RepositoryBase{TEntity,TDbContext}.UpdateAsync(TEntity)"/>
        public override async Task<(bool success, Guid id, bool updated)> UpdateAsync(TrainingSession entity)
        {
            bool saveSuccess = false;
            using EntityLoadLock.Releaser loadLock = EntityLoadLock.Shared.Lock();
            EntityEntry<TrainingSession> entry = DbContext.Update(entity);
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
            return (success: saveSuccess, id: entity.Id, updated: entry.State == EntityState.Modified);
        }
    }
}
