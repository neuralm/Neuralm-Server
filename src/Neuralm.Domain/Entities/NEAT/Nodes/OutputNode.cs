using System.Collections.Generic;

namespace Neuralm.Domain.Entities.NEAT.Nodes
{
    /// <summary>
    /// Represents the <see cref="OutputNode"/> class.
    /// </summary>
    public class OutputNode : Node
    {
        /// <summary>
        /// EFCore Entity constructor IGNORE!
        /// </summary>
        public OutputNode() { }

        /// <summary>
        /// Initializes an instance of the <see cref="OutputNode"/> class.
        /// </summary>
        /// <param name="nodeIdentifier">The node identifier.</param>
        public OutputNode(uint nodeIdentifier) : base(nodeIdentifier) { }

        /// <summary>
        /// Gets and sets the list of <see cref="Organism"/> to <see cref="OutputNode"/> relations. 
        /// </summary>
        public virtual IList<OrganismOutputNode> OrganismOutputNodes { get; set; }
    }
}
