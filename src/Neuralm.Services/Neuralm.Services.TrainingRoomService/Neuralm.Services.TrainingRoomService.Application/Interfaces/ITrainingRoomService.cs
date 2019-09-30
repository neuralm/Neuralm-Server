using Neuralm.Services.Common.Application.Interfaces;
using Neuralm.Services.TrainingRoomService.Messages.Dtos;

namespace Neuralm.Services.TrainingRoomService.Application.Interfaces
{
    /// <summary>
    /// Represents the <see cref="ITrainingRoomService"/> interface.
    /// </summary>
    public interface ITrainingRoomService : IService<TrainingRoomDto>
    {
        ///// <summary>
        ///// Creates a training room asynchronously.
        ///// </summary>
        ///// <param name="createTrainingRoomRequest">The create training room request.</param>
        ///// <returns>Returns an awaitable <see cref="Task"/> with parameter type <see cref="CreateTrainingRoomResponse"/>.</returns>
        //Task<CreateTrainingRoomResponse> CreateTrainingRoomAsync(CreateTrainingRoomRequest createTrainingRoomRequest);

        ///// <summary>
        ///// Gets the enabled training rooms asynchronously.
        ///// </summary>
        ///// <param name="getEnabledTrainingRoomsRequest">The get enabled training rooms request.</param>
        ///// <returns>Returns an awaitable <see cref="Task"/> with parameter type <see cref="GetEnabledTrainingRoomsResponse"/>.</returns>
        //Task<GetEnabledTrainingRoomsResponse> GetEnabledTrainingRoomsAsync(GetEnabledTrainingRoomsRequest getEnabledTrainingRoomsRequest);
    }
}
