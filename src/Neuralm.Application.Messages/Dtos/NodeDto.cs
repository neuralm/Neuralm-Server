using System;
using Neuralm.Domain.Entities.NEAT.Nodes;

namespace Neuralm.Application.Messages.Dtos
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
