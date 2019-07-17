using System;

namespace Neuralm.Domain.Entities.NEAT
{
    /// <summary>
    /// The ConnectionGene class; used for connecting <see cref="Node"/>s in a <see cref="Brain"/>.
    /// </summary>
    public class ConnectionGene : IEquatable<ConnectionGene>
    {
        /// <summary>
        /// Gets and sets the id.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets and sets the brain id.
        /// </summary>
        public Guid BrainId { get; private set; }

        /// <summary>
        /// Gets and sets the in id.
        /// </summary>
        public uint InId { get; private set; }

        /// <summary>
        /// Gets and sets the out id.
        /// </summary>
        public uint OutId { get; private set; }

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
        public Node InNode { get; private set; }

        /// <summary>
        /// Gets and sets the out node.
        /// </summary>
        public Node OutNode { get; private set; }

        /// <summary>
        /// EFCore entity constructor IGNORE!
        /// </summary>
        protected ConnectionGene()
        {

        }

        /// <summary>
        /// Creates a connection gene going from the node with the given inID to the node with the given outID with a given weight.
        /// </summary>
        /// <param name="brainId">The id of the brain.</param>
        /// <param name="inId">The id of in node.</param>
        /// <param name="outId">The id of the out node.</param>
        /// <param name="weight">The weight.</param>
        /// <param name="innovationNumber">The innovation number.</param>
        /// <param name="enabled">Whether this gene is enabled or not. Default is true.</param>
        public ConnectionGene(Guid brainId, uint inId, uint outId, double weight, uint innovationNumber, bool enabled = true)
        {
            Id = Guid.NewGuid();
            BrainId = brainId;
            InId = inId;
            OutId = outId;
            Weight = weight;
            InnovationNumber = innovationNumber;
            Enabled = enabled;
            InNode = null;
            OutNode = null;
        }

        /// <summary>
        /// Adds a reference to the node object's with the in and out id.
        /// </summary>
        /// <param name="organism">The organism.</param>
        public void LoadNodes(Brain organism)
        {
            InNode = organism.GetOrCreateNode(InId);
            OutNode = organism.GetOrCreateNode(OutId);
            OutNode.Dependencies.Add(this); 
        }

        /// <summary>
        /// Clones this connection gene to produce a gene that equals the original gene but is not the same instance.
        /// </summary>
        /// <returns>Returns a new <see cref="ConnectionGene"/> with the same inID, outID, weight, innovation number and enabled.</returns>
        public ConnectionGene Clone()
        {
            return new ConnectionGene(BrainId, InId, OutId, Weight, InnovationNumber, Enabled);
        }

        /// <summary>
        /// Checks if the other gene is the same connection as this gene.
        /// Takes into account:
        ///     The outID
        ///     The inID
        ///     The innovationNumber
        ///     The weight
        ///     Whether it is enabled
        /// </summary>
        /// <param name="other">The other connection gene.</param>
        /// <returns>Returns <c>true</c> if equals; otherwise, <c>false</c>.</returns>
        public bool Equals(ConnectionGene other)
        {
            return other != null && InId == other.InId &&
                   OutId == other.OutId &&
                   InnovationNumber == other.InnovationNumber &&
                   Weight.Equals(other.Weight) &&
                   Enabled == other.Enabled;
        }

        /// <summary>
        /// Checks if an object is the same as this gene.
        /// </summary>
        /// <param name="obj">The other object.</param>
        /// <returns>Returns <c>true</c> if obj is of type <see cref="ConnectionGene"/>and <see cref="Equals(Neuralm.Domain.Entities.NEAT.ConnectionGene)"/> returns <c>true</c>, otherwise, <c>false</c>.</returns>
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
            return $"{InId} -> {OutId};  I: {InnovationNumber} E:{Enabled} W:{Weight} B:{InNode != null && OutNode != null}";
        }

        /// <summary>
        /// Gets the hash code.
        /// </summary>
        /// <returns>Returns the hash code.</returns>
        public override int GetHashCode()
        {
            int hashCode = 1721005899;
            hashCode = hashCode * -1521134295 + InId.GetHashCode();
            hashCode = hashCode * -1521134295 + OutId.GetHashCode();
            hashCode = hashCode * -1521134295 + InnovationNumber.GetHashCode();
            hashCode = hashCode * -1521134295 + Weight.GetHashCode();
            hashCode = hashCode * -1521134295 + Enabled.GetHashCode();
            return hashCode;
        }
    }
}
