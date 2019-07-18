using System.Threading.Tasks;
using Neuralm.Application.Messages.Requests;
using Neuralm.Application.Messages.Responses;

namespace Neuralm.Application.Interfaces
{
    /// <summary>
    /// Represents the <see cref="ITrainingRoomService"/> interface.
    /// </summary>
    public interface ITrainingRoomService : IService
    {
        /// <summary>
        /// Creates a training room asynchronously.
        /// </summary>
        /// <param name="createTrainingRoomRequest">The create training room request.</param>
        /// <returns>Returns an awaitable <see cref="Task"/> with parameter type <see cref="CreateTrainingRoomResponse"/>.</returns>
        Task<CreateTrainingRoomResponse> CreateTrainingRoomAsync(CreateTrainingRoomRequest createTrainingRoomRequest);

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
        /// Gets the enabled training rooms asynchronously.
        /// </summary>
        /// <param name="getEnabledTrainingRoomsRequest">The get enabled training rooms request.</param>
        /// <returns>Returns an awaitable <see cref="Task"/> with parameter type <see cref="GetEnabledTrainingRoomsResponse"/>.</returns>
        Task<GetEnabledTrainingRoomsResponse> GetEnabledTrainingRoomsAsync(GetEnabledTrainingRoomsRequest getEnabledTrainingRoomsRequest);

        /// <summary>
        /// Gets the organisms for a training session asynchronously.
        /// </summary>
        /// <param name="getOrganismsRequest">The get organism request.</param>
        /// <returns>Returns an awaitable <see cref="Task"/> with parameter type <see cref="GetOrganismsResponse"/>.</returns>
        Task<GetOrganismsResponse> GetOrganismsAsync(GetOrganismsRequest getOrganismsRequest);

        /// <summary>
        /// Posts the scores of the organisms for a training session asynchronously.
        /// </summary>
        /// <param name="postOrganismsScoreRequest">The post organisms score request.</param>
        /// <returns>Returns an awaitable <see cref="Task"/> with parameter type <see cref="PostOrganismsScoreResponse"/>.</returns>
        Task<PostOrganismsScoreResponse> PostOrganismsScoreAsync(PostOrganismsScoreRequest postOrganismsScoreRequest);
    }
}
