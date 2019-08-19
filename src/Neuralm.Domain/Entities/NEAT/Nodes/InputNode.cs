using System.Collections.Generic;

namespace Neuralm.Domain.Entities.NEAT.Nodes
{
    /// <summary>
    /// Represents the <see cref="InputNode"/> class.
    /// </summary>
    public class InputNode : Node
    {
        /// <summary>
        /// EFCore Entity constructor IGNORE!
        /// </summary>
        public InputNode() { }

        /// <summary>
        /// Initializes an instance of the <see cref="InputNode"/> class.
        /// </summary>
        /// <param name="nodeIdentifier">The node identifier.</param>
        public InputNode(uint nodeIdentifier) : base(nodeIdentifier) { }

        /// <summary>
        /// Gets and sets the list of <see cref="Organism"/> to <see cref="InputNode"/> relations. 
        /// </summary>
        public virtual IList<OrganismInputNode> OrganismInputNodes { get; set; }
    }
}
