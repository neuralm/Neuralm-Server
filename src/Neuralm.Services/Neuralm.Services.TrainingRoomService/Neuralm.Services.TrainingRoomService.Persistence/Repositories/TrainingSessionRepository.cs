using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Neuralm.Services.Common.Exceptions;
using Neuralm.Services.Common.Persistence;
using Neuralm.Services.Common.Persistence.EFCore;
using Neuralm.Services.Common.Persistence.EFCore.Abstractions;
using Neuralm.Services.TrainingRoomService.Domain;
using Neuralm.Services.TrainingRoomService.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using Neuralm.Services.Common.Domain;
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
                Console.WriteLine(new CreatingEntityFailedException($"The entity of type {typeof(TrainingSession).Name} could not be created.", ex));
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
                Console.WriteLine(new CreatingEntityFailedException($"The entity of type {typeof(TrainingSession).Name} could not be created.", ex));
            }
        }

        /// <inheritdoc cref="ITrainingSessionRepository.UpdateOrganismsAsync(TrainingSession)"/>
        public async Task UpdateOrganismsAsync(TrainingSession trainingSession)
        {
            using EntityLoadLock.Releaser loadLock = EntityLoadLock.Shared.Lock();
            try
            {
                //foreach (Species species in trainingSession.TrainingRoom.Species)
                //{
                //    if (species.GetType() != typeof(Species))
                //    {
                //        DbContext.Entry(species).State = EntityState.Unchanged;
                //    }
                //    else
                //    {
                //        Console.WriteLine($"{species.GetType()} -> {DbContext.Entry(species).State} | Organisms: {species.Organisms.Count}");
                //        DbContext.Entry(species).State = EntityState.Added;
                //        Console.WriteLine($"{species.GetType()} -> {DbContext.Entry(species).State} | Organisms: {species.Organisms.Count}");
                //        foreach (Organism organism in species.Organisms)
                //        {
                //            Console.WriteLine($"{organism.GetType()} -> {DbContext.Entry(organism).State} | ConnectionGenes: {organism.ConnectionGenes.Count}");
                //            DbContext.Entry(organism).State = EntityState.Added;
                //            Console.WriteLine($"{organism.GetType()} -> {DbContext.Entry(organism).State} | ConnectionGenes: {organism.ConnectionGenes.Count}");
                //            foreach (ConnectionGene connectionGene in organism.ConnectionGenes)
                //            {
                //                Console.WriteLine($"{connectionGene.GetType()} -> {DbContext.Entry(connectionGene).State} | ConnectionGene: {connectionGene}");
                //                DbContext.Entry(connectionGene).State = EntityState.Added;
                //                Console.WriteLine($"{connectionGene.GetType()} -> {DbContext.Entry(connectionGene).State} | ConnectionGene: {connectionGene}");
                //            }
                //        }
                //    }
                //}


                //// TODO: implement fix
                IEnumerable<EntityEntry> changes = from e in DbContext.ChangeTracker.Entries()
                                                   where e.State != EntityState.Unchanged
                                                   select e;

                foreach (EntityEntry change in changes)
                {
                    if (!(change.Entity is IEntity item))
                        throw new NullReferenceException($"Entity: {change}");
                    Console.WriteLine($"{change.Entity.GetType().Name} {change.State}: {item.Id}");
                    Console.WriteLine(change.Entity.ToString());
                    switch (change.State)
                    {
                        case EntityState.Added:
                            break;
                        case EntityState.Modified:
                            {
                                foreach (IProperty property in change.OriginalValues.Properties)
                                {
                                    object original = change.OriginalValues[property];
                                    object current = change.CurrentValues[property];
                                    if (Equals(original, current))
                                        continue;
                                    string originalString = original is null ? "NULL" : original.ToString();
                                    string currentString = current is null ? "NULL" : current.ToString();
                                    Console.WriteLine($"\t{property.Name}: {originalString} --> {currentString}");
                                }
                                break;
                            }
                        case EntityState.Deleted:
                            break;
                        case EntityState.Detached:
                            break;
                        case EntityState.Unchanged:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                await DbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(new CreatingEntityFailedException($"The entity of type {typeof(TrainingSession).Name} could not be created.", ex));
            }
        }
    }
}
