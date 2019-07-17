using System.Collections.Generic;

namespace Neuralm.Domain.Entities.NEAT
{
    /// <summary>
    /// Represents the <see cref="Node"/> class.
    /// </summary>
    public class Node
    {
        /// <summary>
        /// Gets the list of dependencies.
        /// </summary>
        public List<ConnectionGene> Dependencies { get; }

        /// <summary>
        /// Gets the id.
        /// </summary>
        public uint Id { get; }

        /// <summary>
        /// Gets the layer.
        /// </summary>
        public uint Layer { get; private set; }

        /// <summary>
        /// Gets the node type.
        /// </summary>
        public NodeType NodeType { get; }

        /// <summary>
        /// Initializes an instance of the <see cref="Node"/> class with a given id.
        /// </summary>
        /// <param name="id">The node's id/</param>
        /// <param name="type">The node type.</param>
        public Node(uint id, NodeType type)
        {
            Layer = uint.MinValue;
            Dependencies = new List<ConnectionGene>();
            Id = id;
            NodeType = type;
        }

        /// <summary>
        /// Sets the layer of this node.
        /// Will only be set if either force is true, or the layer is higher than its current layer.
        /// </summary>
        /// <param name="layer">The new layer value.</param>
        /// <param name="force">If <c>true</c> it will always set the new layer, else only if it is bigger than the current layer.</param>
        public void SetLayer(uint layer, bool force = false)
        {
            Layer = force ? layer : (layer > Layer ? layer : Layer);

            if (!force)
            {
                foreach (ConnectionGene con in Dependencies)
                {
                    con.InNode.SetLayer(Layer + 1);
                }
            }
        }

        /// <summary>
        /// Checks whether the object is the same as this node.
        /// </summary>
        /// <param name="obj">The other object.</param>
        /// <returns>Returns <c>true</c> if they are the same; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return obj is Node node && Equals(node);
        }

        /// <summary>
        /// Checks whether the other node is the same as this node.
        /// Only the case if the id's are the same.
        /// </summary>
        /// <param name="other">The other node.</param>
        /// <returns>Returns <c>true</c> if they are the same; otherwise, <c>false</c>.</returns>
        private bool Equals(Node other)
        {
            return other.Id.Equals(Id);
        }

        /// <summary>
        /// Gets the hash code.
        /// </summary>
        /// <returns>Returns the hash code.</returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    /// <summary>
    /// Represents the <see cref="NodeType"/> enumeration.
    /// </summary>
    public enum NodeType
    {
        Input, Hidden, Output
    }
}