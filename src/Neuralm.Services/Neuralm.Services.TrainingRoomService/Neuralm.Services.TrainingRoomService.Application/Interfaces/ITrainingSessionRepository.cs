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
        /// <returns>Returns <c>true</c> If the training session's first generation is successfully inserted; otherwise, <c>false</c>.</returns>
        Task InsertFirstGenerationAsync(TrainingSession trainingSession);
    }
}