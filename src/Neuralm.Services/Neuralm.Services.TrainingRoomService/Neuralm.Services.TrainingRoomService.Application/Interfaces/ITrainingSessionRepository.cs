using Neuralm.Services.Common.Application.Interfaces;
using Neuralm.Services.TrainingRoomService.Domain;
using System.Threading.Tasks;

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
        /// Inserts the leased organisms asynchronously.
        /// </summary>
        /// <param name="trainingSession">The training session.</param>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        Task InsertLeasedOrganismsAsync(TrainingSession trainingSession);
        
        /// <summary>
        /// Updates the organisms asynchronously.
        /// </summary>
        /// <param name="trainingSession">The training session.</param>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        Task UpdateOrganismsAsync(TrainingSession trainingSession);

        /// <summary>
        /// Marks the organism for removal.
        /// </summary>
        /// <param name="organism">The organism to mark.</param>
        void MarkOrganismForRemoval(Organism organism);

        /// <summary>
        /// Marks the species for removal.
        /// </summary>
        /// <param name="species">The species to mark.</param>
        void MarkSpeciesForRemoval(Species species);
    }
}