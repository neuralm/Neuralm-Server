using System;

namespace Neuralm.Services.TrainingRoomService.Messages.Dtos
{
    /// <summary>
    /// Represents the data transfer object for the Node class.
    /// </summary>
    public class NodeDto
    {
        /// <summary>
        /// Gets and sets an id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets and sets the layer.
        /// </summary>
        public uint Layer { get; set; }

        /// <summary>
        /// Gets and sets the node identifier.
        /// </summary>
        public uint NodeIdentifier { get; set; }
    }
}
