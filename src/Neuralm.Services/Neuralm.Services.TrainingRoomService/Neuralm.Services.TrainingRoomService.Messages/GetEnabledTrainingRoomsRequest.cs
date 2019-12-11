using Neuralm.Services.Common.Messages;
using Neuralm.Services.Common.Messages.Abstractions;

namespace Neuralm.Services.TrainingRoomService.Messages
{
    /// <summary>
    /// Represents the <see cref="GetEnabledTrainingRoomsRequest"/> class.
    /// </summary>
    [Message("GetAll", "/", typeof(GetEnabledTrainingRoomsResponse))]
    public class GetEnabledTrainingRoomsRequest : Request
    {
        
    }
}
