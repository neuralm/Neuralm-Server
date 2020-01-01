using System.Threading.Tasks;
using Neuralm.Services.Common.Application.Interfaces;
using Neuralm.Services.TrainingRoomService.Messages;
using Neuralm.Services.TrainingRoomService.Messages.Dtos;

namespace Neuralm.Services.TrainingRoomService.Application.Interfaces
{
    /// <summary>
    /// Represents the <see cref="ITrainingSessionService"/> interface.
    /// </summary>
    public interface ITrainingSessionService : IService<TrainingSessionDto>
    {
        /// <summary>
        /// Starts a training session asynchronously.
        /// </summary>
        /// <param name="startTrainingSessionRequest">The start training session request.</param>
        /// <returns>Returns an awaitable <see cref="Task"/> with parameter type <see cref="StartTrainingSessionResponse"/>.</returns>
        Task<StartTrainingSessionResponse> StartTrainingSessionAsync(StartTrainingSessionRequest startTrainingSessionRequest);

        /// <summary>
        /// Ends a training session asynchronously.
        /// </summary>
        /// <param name="endTrainingSessionRequest"></param>
        /// <returns>Returns an awaitable <see cref="Task"/> with parameter type <see cref="EndTrainingSessionResponse"/>.</returns>
        Task<EndTrainingSessionResponse> EndTrainingSessionAsync(EndTrainingSessionRequest endTrainingSessionRequest);

        /// <summary>
        /// Gets the organisms for a training session asynchronously.
        /// </summary>
        /// <param name="getOrganismsRequest">The get organism request.</param>
        /// <returns>Returns an awaitable <see cref="Task"/> with parameter type <see cref="GetOrganismsResponse"/>.</returns>
        Task<GetOrganismsResponse> GetOrganismsAsync(GetOrganismsRequest getOrganismsRequest);

        ///// <summary>
        ///// Posts the scores of the organisms for a training session asynchronously.
        ///// </summary>
        ///// <param name="postOrganismsScoreRequest">The post organisms score request.</param>
        ///// <returns>Returns an awaitable <see cref="Task"/> with parameter type <see cref="PostOrganismsScoreResponse"/>.</returns>
        //Task<PostOrganismsScoreResponse> PostOrganismsScoreAsync(PostOrganismsScoreRequest postOrganismsScoreRequest);
    }
}
