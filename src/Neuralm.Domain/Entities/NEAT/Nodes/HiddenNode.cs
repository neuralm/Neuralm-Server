namespace Neuralm.Domain.Entities.NEAT.Nodes
{
    /// <summary>
    /// Represents the <see cref="HiddenNode"/> class.
    /// </summary>
    public class HiddenNode : Node
    {
        /// <summary>
        /// Initializes an instance of the <see cref="HiddenNode"/> class.
        /// </summary>
        /// <param name="nodeIdentifier"></param>
        public HiddenNode(uint nodeIdentifier) : base(nodeIdentifier) { }
    }
}
