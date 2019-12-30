using Neuralm.Services.Common.Messages;

namespace Neuralm.Services.TrainingRoomService.Messages
{
    /// <summary>
    /// Represents the <see cref="PaginateTrainingRoomRequest"/> class.
    /// </summary>
    [Message("Get", "/", typeof(PaginateTrainingRoomResponse))]
    public class PaginateTrainingRoomRequest : PaginationRequest
    {
        
    }
}