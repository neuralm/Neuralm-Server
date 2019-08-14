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

        ///// <inheritdoc cref="RepositoryBase{TEntity,TDbContext}.FindSingleOrDefaultAsync"/>
        //public override async Task<TrainingSession> FindSingleOrDefaultAsync(Expression<Func<TrainingSession, bool>> predicate)
        //{
        //    using EntityLoadLock.Releaser loadLock = EntityLoadLock.Shared.Lock();
        //    TrainingSession x = await DbContext.TrainingSessions.Where(predicate).SingleOrDefaultAsync();
        //    //DbContext.Entry(x).Reference(p => p.TrainingRoom).Load();
        //    //DbContext.Entry(x).Collection(p => p.LeasedOrganisms).Load();
        //    //DbContext.Entry(x.TrainingRoom).Reference(p => p.TrainingRoomSettings).Load();
        //    //DbContext.Entry(x.TrainingRoom).Collection(p => p.AuthorizedTrainers).Load();
        //    //DbContext.Entry(x.TrainingRoom).Collection(p => p.Species).Load();
        //    //DbContext.Entry(x.TrainingRoom).Collection(p => p.Brains).Load();
        //    //DbContext.Entry(x.TrainingRoom).Reference(p => p.Owner).Load();

        //    return x; // await TrainingSessionSetWithInclude().Where(predicate).SingleOrDefaultAsync();
        //}

        //public override async Task<bool> UpdateAsync(TrainingSession entity)
        //{
        //    using (EntityLoadLock.Releaser loadLock = EntityLoadLock.Shared.Lock())
        //    {
        //        bool saveSuccess = false;
        //        try
        //        {
        //            //DbContext.Entry(entity.LeasedOrganisms).State = EntityState.Modified;
        //            //foreach (Species species in entity.TrainingRoom.Species)
        //            //{
        //            //    DbContext.Entry(species).State = EntityState.Modified;
        //            //}
        //            //if (!entity.LeasedOrganisms.Any())
        //            //    foreach (Species species in entity.TrainingRoom.Species)
        //            //    {
        //            //        DbContext.Entry(species).State = EntityState.Modified;
        //            //    }
        //            DbContext.Update(entity);
        //            int saveResult = await DbContext.SaveChangesAsync();
        //            saveSuccess = Convert.ToBoolean(saveResult);
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(new UpdatingEntityFailedException($"The entity of type {typeof(TrainingSession).Name} failed to update.", ex));
        //        }
        //        return saveSuccess;
        //    }
        //}

        private IQueryable<TrainingSession> TrainingSessionSetWithInclude()
        {
            return DbContext.Set<TrainingSession>()
                .Include("LeasedOrganisms")
                .Include("TrainingRoom.TrainingRoomSettings")
                .Include("TrainingRoom.AuthorizedTrainers")
                .Include("TrainingRoom.Species.Organisms.Brain")
                .Include("TrainingRoom.Species.Organisms.TrainingRoom")
                .Include("TrainingRoom.Species.LastGenerationOrganisms.Brain")
                .Include("TrainingRoom.Species.LastGenerationOrganisms.TrainingRoom");
        }
    }
}
