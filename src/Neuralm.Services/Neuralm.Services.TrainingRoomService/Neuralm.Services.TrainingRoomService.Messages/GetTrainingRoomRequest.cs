using System;
using Neuralm.Services.Common.Messages;
using Neuralm.Services.Common.Messages.Abstractions;
using Neuralm.Services.Common.Messages.Interfaces;

namespace Neuralm.Services.TrainingRoomService.Messages
{
    /// <summary>
    /// Represents the <see cref="GetTrainingRoomRequest"/> class.
    /// </summary>
    [Message("Get", "/", typeof(GetTrainingRoomResponse))]
    public class GetTrainingRoomRequest : Request, IGetRequest
    {
        /// <inheritdoc cref="IGetRequest.GetId"/>
        public Guid GetId { get; set; }
    }
}