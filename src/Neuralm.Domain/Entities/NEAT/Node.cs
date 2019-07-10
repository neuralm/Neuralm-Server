using System.Collections.Generic;

namespace Neuralm.Domain.Entities.NEAT
{
    public class Node
    {
        public List<ConnectionGene> Dependencies { get; }
        public int Id { get; }
        public int Layer { get; private set; }

        public NodeType NodeType { get; }

        /// <summary>
        /// Create a node with te given id
        /// </summary>
        /// <param name="id">The node's ID</param>
        /// <param name="type">The type of node</param>
        public Node(int id, NodeType type)
        {
            Layer = int.MinValue;
            Dependencies = new List<ConnectionGene>();
            Id = id;
            NodeType = type;
        }

        /// <summary>
        /// Set the layer of this node.
        /// Will only be set if either force is true, or the layer is higher than its current layer.
        /// </summary>
        /// <param name="layer">The new layer value</param>
        /// <param name="force">If true it will always set the new layer, else only if it is bigger than the current layer</param>
        public void SetLayer(int layer, bool force = false)
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
        /// Check whether the object is the same as this node.
        /// </summary>
        /// <param name="obj">The other object</param>
        /// <returns>true if they are the same</returns>
        public override bool Equals(object obj)
        {
            return obj is Node node && Equals(node);
        }

        /// <summary>
        /// Check whether the other node is the same as this node.
        /// Only the case if the ID's are the same
        /// </summary>
        /// <param name="other">The other node</param>
        /// <returns>true if they are the same</returns>
        private bool Equals(Node other)
        {
            return other.Id.Equals(Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public enum NodeType
    {
        Input, Hidden, Output
    }
}