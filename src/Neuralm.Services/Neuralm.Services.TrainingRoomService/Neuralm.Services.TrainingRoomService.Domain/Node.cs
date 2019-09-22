using Neuralm.Services.Common.Domain;
using System;
using System.Diagnostics;

namespace Neuralm.Services.TrainingRoomService.Domain
{
    /// <summary>
    /// Represents the <see cref="Node"/> class.
    /// </summary>
    [DebuggerDisplay("Id = {Id}, Layer = {Layer}, NodeIdentifier = {NodeIdentifier}")]
    public class Node : IEntity
    {
        /// <summary>
        /// Gets and sets the id.
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

        /// <summary>
        /// EFCore Entity constructor IGNORE!
        /// </summary>
        protected Node() { }

        /// <summary>
        /// Initializes an instance of the <see cref="Node"/> class.
        /// </summary>
        /// <param name="nodeIdentifier">The node identifier.</param>
        protected Node(uint nodeIdentifier)
        {
            Id = Guid.NewGuid();
            NodeIdentifier = nodeIdentifier;
        }
    }
}
