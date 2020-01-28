using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Neuralm.Services.Common.Application.Interfaces;
using Neuralm.Services.TrainingRoomService.Domain;

namespace Neuralm.Services.TrainingRoomService.Application.Interfaces
{
    /// <summary>
    /// Represents the <see cref="ITrainingSessionRepository"/> interface.
    /// </summary>
    public interface ITrainingSessionRepository : IRepository<TrainingSession>
    {
        /// <summary>
        /// Inserts the first generation asynchronously.
        /// </summary>
        /// <param name="trainingSession">The training session.</param>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        Task InsertFirstGenerationAsync(TrainingSession trainingSession);
        
        /// <summary>
        /// Updates the organisms asynchronously.
        /// </summary>
        /// <param name="trainingSession">The training session.</param>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        Task UpdateOrganismsAsync(TrainingSession trainingSession);

        Task<TrainingRoom> FindSingleOrDefaultTrainingRoomAsync(Expression<Func<TrainingRoom, bool>> predicate);
        void MarkAsAdded(Organism organism);
    }
}