using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Neuralm.Services.Common.Exceptions;
using Neuralm.Services.Common.Persistence;
using Neuralm.Services.Common.Persistence.EFCore;
using Neuralm.Services.Common.Persistence.EFCore.Abstractions;
using Neuralm.Services.TrainingRoomService.Application.Interfaces;
using Neuralm.Services.TrainingRoomService.Domain;
using Neuralm.Services.TrainingRoomService.Persistence.Contexts;
using System;
using System.Linq;
using System.Threading.Tasks;

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
        /// <param name="logger">The logger.</param>
        public TrainingSessionRepository(
            TrainingRoomDbContext dbContext, 
            IEntityValidator<TrainingSession> entityValidator, 
            ILogger<TrainingRoomDbContext> logger) : base(dbContext, entityValidator, logger)
        {

        }

        /// <inheritdoc cref="RepositoryBase{TEntity,TDbContext}.CreateAsync(TEntity)"/>
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
                CreatingEntityFailedException creatingEntityFailedException = new CreatingEntityFailedException($"The entity of type {typeof(TrainingSession).Name} could not be created.", ex);
                Logger.LogError(creatingEntityFailedException, creatingEntityFailedException.Message);
                throw creatingEntityFailedException;
            }
            return (success: saveSuccess, id: entity.Id);
        }

        /// <inheritdoc cref="ITrainingSessionRepository.InsertFirstGenerationAsync(TrainingSession)"/>
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
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        /// <inheritdoc cref="ITrainingSessionRepository.InsertLeasedOrganismsAsync(TrainingSession)"/>
        public async Task InsertLeasedOrganismsAsync(TrainingSession trainingSession)
        {
            try
            {
                foreach (LeasedOrganism leasedOrganism in trainingSession.LeasedOrganisms)
                {
                    Logger.LogInformation($"{DbContext.Entry(leasedOrganism).State}");
                    if (DbContext.Entry(leasedOrganism).State != EntityState.Unchanged)
                        DbContext.Entry(leasedOrganism).State = EntityState.Added;
                }
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        /// <inheritdoc cref="ITrainingSessionRepository.MarkAsAdded(Organism)"/>
        public void MarkAsAdded(Organism organism)
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

        /// <inheritdoc cref="ITrainingSessionRepository.UpdateOrganismsAsync(TrainingSession)"/>
        public async Task UpdateOrganismsAsync(TrainingSession trainingSession)
        {
            using EntityLoadLock.Releaser loadLock = EntityLoadLock.Shared.Lock();
            try
            {
                foreach (Species species in trainingSession.TrainingRoom.Species)
                {
                    Logger.LogInformation($"{species.GetType()} -> {DbContext.Entry(species).State} | Organisms: {species.Organisms.Count}");
                    foreach (Organism organism in species.Organisms)
                    {
                        bool newGenes = false;
                        foreach (ConnectionGene connectionGene in organism.ConnectionGenes)
                        {
                            if (DbContext.Entry(connectionGene).State != EntityState.Added) 
                                continue;
                            newGenes = true;
                            break;
                        }
                        if (newGenes)
                            DbContext.Entry(organism).State = EntityState.Added;
                    }
                }

                foreach (EntityEntry entityEntry in DbContext.ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Modified && (e.Entity.GetType() == typeof(InputNode) || e.Entity.GetType() == typeof(OutputNode))))
                {
                    DbContext.Entry(entityEntry.Entity).State = EntityState.Added;
                }
                await DbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
