using System;

namespace Neuralm.Application.Messages.Requests
{
    /// <summary>
    /// Represents the <see cref="GetOrganismsRequest"/> class.
    /// </summary>
    public class GetOrganismsRequest : Request
    {
        /// <summary>
        /// Gets the training room id.
        /// </summary>
        public Guid TrainingRoomId { get; }

        /// <summary>
        /// Initializes an instance of the <see cref="GetOrganismsRequest"/> class.
        /// </summary>
        public GetOrganismsRequest(Guid trainingRoomId)
        {
            TrainingRoomId = trainingRoomId;
        }
    }
}
