using System;

namespace Neuralm.Domain.Entities.NEAT
{
    public class ConnectionGene : IEquatable<ConnectionGene>
    {
        public Guid Id { get; private set; }
        public Guid BrainId { get; private set; }
        public uint InId { get; private set; }
        public uint OutId { get; private set; }
        public uint InnovationNumber { get; private set; }
        public double Weight { get; set; }
        public bool Enabled { get; set; }
        public Node InNode { get; private set; }
        public Node OutNode { get; private set; }

        /// <summary>
        /// Create a connection gene going from the node with the given inID to the node with the given outID with a given weight.
        /// </summary>
        /// <param name="brainId">The id of the brain</param>
        /// <param name="inId">The id of in node</param>
        /// <param name="outId">The id of the out node</param>
        /// <param name="weight">The weight</param>
        /// <param name="innovationNumber">The innovation number</param>
        /// <param name="enabled">Whether this gene is enabled or not. Default is true</param>
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
        /// Get a reference to the node object's with the in and out id.
        /// </summary>
        /// <param name="organism"></param>
        public void LoadNodes(Brain organism)
        {
            InNode = organism.GetOrCreateNode(InId);
            OutNode = organism.GetOrCreateNode(OutId);
            OutNode.Dependencies.Add(this); 
        }

        /// <summary>
        /// Clone this connection gene to produce a gene that equals the original gene but is not the same instance.
        /// </summary>
        /// <returns>A new gene with the same inID, outID, weight, innovation number and enabled</returns>
        public ConnectionGene Clone()
        {
            return new ConnectionGene(BrainId, InId, OutId, Weight, InnovationNumber, Enabled);
        }

        /// <summary>
        /// Check if the other gene is the same connection as this gene.
        /// Takes into account:
        ///     The outID
        ///     The inID
        ///     The innovationNumber
        ///     The weight
        ///     Whether it is enabled
        /// </summary>
        /// <param name="other">The other connection gene</param>
        /// <returns>true if equals, else false</returns>
        public bool Equals(ConnectionGene other)
        {
            return other != null && InId == other.InId &&
                   OutId == other.OutId &&
                   InnovationNumber == other.InnovationNumber &&
                   Weight.Equals(other.Weight) &&
                   Enabled == other.Enabled;
        }

        /// <summary>
        /// Check if an object is the same as this gene
        /// </summary>
        /// <param name="obj">The other object</param>
        /// <returns>Returns true; if obj is of type ConnectionGene and Equals is true, else return false.</returns>
        public override bool Equals(object obj)
        {
            return obj is ConnectionGene other && Equals(other);
        }

        /// <summary>
        /// Generate a string containing information about this gene
        /// </summary>
        /// <returns>The string based on information from this gene</returns>
        public override string ToString()
        {
            return $"{InId} -> {OutId};  I: {InnovationNumber} E:{Enabled} W:{Weight} B:{InNode != null && OutNode != null}";
        }

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
