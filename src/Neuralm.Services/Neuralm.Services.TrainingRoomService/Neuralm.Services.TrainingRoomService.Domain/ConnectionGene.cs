using Neuralm.Services.Common.Domain;
using System;
using System.Diagnostics;

namespace Neuralm.Services.TrainingRoomService.Domain
{
    /// <summary>
    /// Represents the <see cref="ConnectionGene"/> class used for connecting <see cref="Node"/>s in a <see cref="Brain"/>.
    /// </summary>
    [DebuggerDisplay("{ToString()}")]
    public class ConnectionGene : IEquatable<ConnectionGene>, IEntity
    {
        /// <summary>
        /// Gets and sets the id.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets and sets the organism id.
        /// </summary>
        public Guid OrganismId { get; private set; }

        /// <summary>
        /// Gets and sets the in id.
        /// </summary>
        public uint InNodeIdentifier { get; private set; }

        /// <summary>
        /// Gets and sets the out id.
        /// </summary>
        public uint OutNodeIdentifier { get; private set; }

        /// <summary>
        /// Gets and sets the innovation number.
        /// </summary>
        public uint InnovationNumber { get; private set; }

        /// <summary>
        /// Gets and sets the weight.
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// Gets and sets the enabled flag.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets and sets the in node.
        /// </summary>
        public virtual Node InNode { get; set; }

        /// <summary>
        /// Gets and sets the out node.
        /// </summary>
        public virtual Node OutNode { get; set; }

        /// <summary>
        /// EFCore entity constructor IGNORE!
        /// </summary>
        protected ConnectionGene() { }

        /// <summary>
        /// Creates a connection gene going from the node with the given inID to the node with the given outID with a given weight.
        /// </summary>
        /// <param name="organismId">The id of the brain.</param>
        /// <param name="inNodeId">The id of in node.</param>
        /// <param name="outNodeId">The id of the out node.</param>
        /// <param name="weight">The weight.</param>
        /// <param name="innovationNumber">The innovation number.</param>
        /// <param name="enabled">Whether this gene is enabled or not. Default is true.</param>
        public ConnectionGene(Guid organismId, uint innovationNumber, uint inNodeId, uint outNodeId, double weight, bool enabled = true)
        {
            OrganismId = organismId;
            InnovationNumber = innovationNumber;
            InNodeIdentifier = inNodeId;
            OutNodeIdentifier = outNodeId;
            Weight = weight;
            Enabled = enabled;
        }

        /// <summary>
        /// Clones this connection gene to produce a gene that equals the original gene but is not the same instance.
        /// </summary>
        /// <returns>Returns a new <see cref="ConnectionGene"/> with the same inID, outID, weight, innovation number and enabled.</returns>
        public ConnectionGene Clone(Guid organismId)
        {
            return new ConnectionGene(organismId, InnovationNumber, InNodeIdentifier, OutNodeIdentifier, Weight, Enabled);
        }

        /// <summary>
        /// Checks if the other gene is the same connection as this gene.
        /// Takes into account:
        ///     The OutNodeIdentifier
        ///     The InNodeIdentifier
        ///     The InnovationNumber
        ///     The Weight
        ///     Whether it is Enabled
        /// </summary>
        /// <param name="other">The other connection gene.</param>
        /// <returns>Returns <c>true</c> if equals; otherwise, <c>false</c>.</returns>
        public bool Equals(ConnectionGene other)
        {
            return other != null &&
                   InNodeIdentifier == other.InNodeIdentifier &&
                   OutNodeIdentifier == other.OutNodeIdentifier &&
                   InnovationNumber == other.InnovationNumber &&
                   Weight == other.Weight &&
                   Enabled == other.Enabled;
        }

        /// <summary>
        /// Checks if an object is the same as this gene.
        /// </summary>
        /// <param name="obj">The other object.</param>
        /// <returns>Returns <c>true</c> if obj is of type <see cref="ConnectionGene"/> and if <see cref="Equals(ConnectionGene)"/> returns <c>true</c>, otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return obj is ConnectionGene other && Equals(other);
        }

        /// <summary>
        /// Generates a string containing information about this gene.
        /// </summary>
        /// <returns>Returns a string based on information from this gene.</returns>
        public override string ToString()
        {
            return $"{InNodeIdentifier} -> {OutNodeIdentifier};  I: {InnovationNumber} E:{Enabled} W:{Weight} B:{InNode != null && OutNode != null}";
        }
    }
}
