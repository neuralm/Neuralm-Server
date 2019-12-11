using Neuralm.Services.Common.Messages.Abstractions;
using Neuralm.Services.TrainingRoomService.Messages.Dtos;

namespace Neuralm.Services.TrainingRoomService.Messages
{
    /// <summary>
    /// Represents the <see cref="GetTrainingRoomResponse"/> class.
    /// </summary>
    public class GetTrainingRoomResponse : Response
    {
        /// <summary>
        /// Gets and sets the training room.
        /// </summary>
        public TrainingRoomDto TrainingRoom { get; set; }
        
        /// <summary>
        /// Initializes an instance of the <see cref="GetTrainingRoomResponse"/> class.
        /// SERIALIZATION CONSTRUCTOR.
        /// </summary>
        public GetTrainingRoomResponse()
        {
            
        }
    }
}