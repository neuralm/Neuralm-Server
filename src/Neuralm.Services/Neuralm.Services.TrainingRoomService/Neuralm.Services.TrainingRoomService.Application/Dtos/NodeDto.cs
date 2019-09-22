using Neuralm.Services.TrainingRoomService.Domain;
using System;

namespace Neuralm.Services.TrainingRoomService.Application.Dtos
{
    /// <summary>
    /// Represents the data transfer object for the <see cref="Node"/> class.
    /// </summary>
    public class NodeDto
    {
        /// <inheritdoc cref="Node.Id"/>
        public Guid Id { get; set; }

        /// <inheritdoc cref="Node.Layer"/>
        public uint Layer { get; set; }

        /// <inheritdoc cref="Node.NodeIdentifier"/>
        public uint NodeIdentifier { get; set; }
    }
}
