using System;
using System.Collections.Generic;
using System.Linq;

namespace Neuralm.Domain.Entities.NEAT
{
    public class Brain
    {
        private List<ConnectionGene> _genes;
        private readonly Dictionary<uint, Node> _nodes;
        private readonly List<Node> _outputNodes;
        private readonly List<Node> _inputNodes;
        private readonly uint _outputCount;
        private readonly uint _inputCount;
        private readonly List<ConnectionGene> _childGenes;
        private uint _maxInnovation;

        public double Score { get; set; }
        public virtual IReadOnlyList<ConnectionGene> Genes => _genes;
        public Guid Id { get; private set; }
        public Guid TrainingRoomId { get; private set; }
        public virtual TrainingRoom TrainingRoom { get; private set; }

        /// <summary>
        /// EFCore entity constructor IGNORE!
        /// </summary>
        protected Brain()
        {
            
        }

        /// <summary>
        /// Create a new brain with a set amount of input and output nodes
        /// </summary>
        /// <param name="inputCount">The amount of input nodes</param>
        /// <param name="outputCount">The amount of output nodes</param>
        /// <param name="trainingRoom">The training room this brain is a part of</param>
        public Brain(uint inputCount, uint outputCount, TrainingRoom trainingRoom)
        {
            TrainingRoomId = trainingRoom.Id;
            TrainingRoom = trainingRoom;
            _genes = new List<ConnectionGene>();
            _nodes = new Dictionary<uint, Node>();
            _outputNodes = new List<Node>();
            _inputNodes = new List<Node>();
            _inputCount = inputCount;
            _outputCount = outputCount;
            _childGenes = new List<ConnectionGene>();

            if (Id == Guid.Empty)
                Id = Guid.NewGuid();

            for (uint i = 0; i < inputCount; i++)
            {
                Node node = new Node(i, NodeType.Input);
                _inputNodes.Add(node);
                _nodes[i] = node;
            }

            for (uint i = 0; i < outputCount; i++)
            {
                Node node = new Node(inputCount + i, NodeType.Output);
                _outputNodes.Add(node);
                _nodes[inputCount + i] = node;
            }

            trainingRoom.IncreaseNodeIdTo(inputCount + outputCount);
        }

        /// <summary>
        /// Create a brain with the passed in genes with a set amount of input and output nodes
        /// </summary>
        /// <param name="id">The id for the brain</param>
        /// <param name="inputCount">The amount of input nodes</param>
        /// <param name="outputCount">The amount of output nodes</param>
        /// <param name="trainingRoom">The training room this brain is a part of</param>
        /// <param name="genes">The genes to create the brain out of</param>
        public Brain(Guid id, uint inputCount, uint outputCount, TrainingRoom trainingRoom, IEnumerable<ConnectionGene> genes) : this(inputCount, outputCount, trainingRoom)
        {
            Id = id;
            foreach (ConnectionGene gene in genes)
            {
                _genes.Add(gene.Clone());
                _maxInnovation = Math.Max(gene.InnovationNumber, _maxInnovation);
            }
        }

        /// <summary>
        /// Create a brain based on the genes of 2 parent brains. 
        /// For each gene in parent1 we check if parent2 has a gene with the same innovation number, if this is the case we randomly choose a parent to get the gene from.
        /// If a gene only exists in one parent we add it no matter what.
        /// </summary>
        /// <param name="parent2Brain">The other parent</param>
        /// <returns>A child brain based on the genes of this brain and the passed brain</returns>
        public Brain Crossover(Brain parent2Brain)
        {
            _childGenes.Clear();
            List<ConnectionGene> parent2 = new List<ConnectionGene>(parent2Brain._genes);

            foreach (ConnectionGene gene in _genes)
            {
                ConnectionGene geneToRemove = default;
                ConnectionGene geneToAdd = gene;

                ConnectionGene gene2 = parent2Brain.Genes.SingleOrDefault(gen => gen.InnovationNumber == gene.InnovationNumber);
                if(gene2 != default)
                {
                    geneToRemove = gene2;
                    geneToAdd = TrainingRoom.Random.NextDouble() < 0.5 ? gene : gene2;
                }

                geneToAdd = geneToAdd.Clone();

                if (geneToRemove != default)
                    parent2.Remove(geneToRemove);

                if(!geneToAdd.Enabled && TrainingRoom.Random.NextDouble() < TrainingRoom.TrainingRoomSettings.EnableConnectionChance)
                {
                    geneToAdd.Enabled = true;
                }

                _childGenes.Add(geneToAdd);
            }

            _childGenes.AddRange(parent2);
            return new Brain(Guid.NewGuid(), _inputCount, _outputCount, TrainingRoom, _childGenes);
        }

        /// <summary>
        /// Get the node with the given id, or create it if there is no node with this id.
        /// </summary>
        /// <param name="id">The node's ID</param>
        /// <returns></returns>
        public Node GetOrCreateNode(uint id)
        {
            return !_nodes.ContainsKey(id) ? _nodes[id] = new Node(id, NodeType.Hidden) : _nodes[id];
        }

        /// <summary>
        /// Clone this brain to produce a brain that equals the other brain but is not the same instance.
        /// </summary>
        /// <param name="newId">Determines whether a new Id should be generated for the clone.</param>
        /// <returns>A new brain with the same genes, training room, inputCount and outputCount</returns>
        public Brain Clone(bool newId = false)
        {
            return new Brain(newId ? Guid.NewGuid() : Id, _inputCount, _outputCount, TrainingRoom, _genes.Select(gene => gene.Clone()).ToList());
        }

        /// <summary>
        /// Rebuilds the brains structure based on the genes.
        /// This will add nodes, create the dependencies and set the layers.
        /// </summary>
        private void RebuildStructure()
        {
            foreach (ConnectionGene gene in _genes)
            {
                gene.LoadNodes(this);
            }

            foreach (Node node in _nodes.Values)
            {
                node.SetLayer(uint.MinValue, true);
            }

            foreach (Node output in _outputNodes)
            {
                output.SetLayer(0);
            }

            foreach (Node input in _inputNodes)
            {
                input.SetLayer(uint.MaxValue, true);
            }
        }

        /// <summary>
        /// Mutate the brain, each time this is called there is a chance to:
        /// - Add a connection
        /// - Add a node
        /// - Change the weight of an existing gene, which can either randomly be reset to a random value or slightly change
        /// These changes can all happen in one call, but the chance that this happens depends on the training room settings
        /// </summary>
        public void Mutate()
        {
            if (TrainingRoom.Random.NextDouble() < TrainingRoom.TrainingRoomSettings.AddConnectionChance)
                AddConnectionMutation();

            if (TrainingRoom.Random.NextDouble() < TrainingRoom.TrainingRoomSettings.AddNodeChance)
                AddNodeMutation();

            if ((TrainingRoom.Random.NextDouble() < TrainingRoom.TrainingRoomSettings.MutateWeightChance) && _genes.Count > 0)
            {
                ConnectionGene connectionGene = _genes.ElementAt(TrainingRoom.Random.Next(_genes.Count));
                if (TrainingRoom.Random.NextDouble() < TrainingRoom.TrainingRoomSettings.WeightReassignChance)
                    connectionGene.Weight = TrainingRoom.Random.NextDouble() * 2 - 1;
                else
                    connectionGene.Weight += (TrainingRoom.Random.NextDouble() * 2 - 1) * 0.1;
            }
        }

        /// <summary>
        /// Adds a new connection between 2 randomly chosen nodes.
        /// The first node can not be an output
        /// The second node cannot be an input node if the first node is an input node, and the layer cannot be the same as the first node
        /// 
        /// If the connection that is going to be created already exists, there will be a maximum of 5 attempts to create a new one.
        /// </summary>
        private void AddConnectionMutation()
        {
            int attemptsDone = 0;
            while (true)
            {
                RebuildStructure(); //Rebuild structure so we get layer information to make sure we don't get circle dependencies

                Node startNode;
                Node endNode;
                do
                {
                    startNode = _nodes.ElementAt(TrainingRoom.Random.Next(_nodes.Count)).Value;
                } while (startNode.NodeType == NodeType.Output);

                do
                {
                    endNode = _nodes.ElementAt(TrainingRoom.Random.Next(_nodes.Count)).Value;
                } while (endNode.Layer == startNode.Layer || (startNode.NodeType == NodeType.Input && endNode.NodeType == NodeType.Input));

                if (endNode.Layer > startNode.Layer)
                {
                    Node temp = endNode;
                    endNode = startNode;
                    startNode = temp;
                }

                if (ConnectionExists(startNode, endNode))
                {
                    if (attemptsDone >= 5)
                        return;
                    attemptsDone += 1;
                    continue;
                }

                ConnectionGene connection = new ConnectionGene(Id, startNode.Id, endNode.Id, TrainingRoom.Random.NextDouble() * 2 - 1, TrainingRoom.GetInnovationNumber(startNode.Id, endNode.Id));
                _genes.Add(connection);
                _maxInnovation = Math.Max(connection.InnovationNumber, _maxInnovation);
                break;
            }
        }

        /// <summary>
        /// Check if a connection between 2 nodes already exists.
        /// This check is directional, this means that ConnectionExists(a,b) is not the same as ConnectionExists(b,a).
        /// </summary>
        /// <param name="startNode">The start node</param>
        /// <param name="endNode">The end node</param>
        /// <returns>Returns true; if there is a connection that goes from the start node to the end node</returns>
        private bool ConnectionExists(Node startNode, Node endNode)
        {
            return _genes.Any(gene => gene.InId == startNode.Id && gene.OutId == endNode.Id);
        }

        /// <summary>
        /// Add a node by selecting a random connection, disabling it and replacing it by 2 genes.
        /// The first gene goes from the original input node, to the new node with a weight of 1.
        /// The second gene goes from the the new node to the old output node with the same weight as the original connection
        /// </summary>
        private void AddNodeMutation()
        {
            if (_genes.Count == 0)
                return;

            // Choose a random connection to add a node into
            ConnectionGene theChosenOne = _genes.ElementAt(TrainingRoom.Random.Next(_genes.Count));

            // Disable it
            theChosenOne.Enabled = false;

            // Generate the new node's id
            uint newNodeId = TrainingRoom.GetAndIncreaseNodeId();

            uint oldInId = theChosenOne.InId;
            uint oldOutId = theChosenOne.OutId;

            // Create a connectionGene from oldIn (a) to new (c) and from new (c) to oldOut (b) 
            ConnectionGene aToC = new ConnectionGene(Id, oldInId, newNodeId, 1, TrainingRoom.GetInnovationNumber(oldInId, newNodeId));
            ConnectionGene cToB = new ConnectionGene(Id, newNodeId, oldOutId, theChosenOne.Weight, TrainingRoom.GetInnovationNumber(newNodeId, oldOutId));

            _genes.Add(aToC);
            _genes.Add(cToB);
            _maxInnovation = Math.Max(Math.Max(cToB.InnovationNumber, aToC.InnovationNumber), _maxInnovation);
        }

        /// <summary>
        /// Check if the other brain is the same species as this brain
        /// </summary>
        /// <param name="other">The other brain</param>
        /// <returns>Returns true; if the other brain is the same species as this brain, else false</returns>
        public bool IsSameSpecies(Brain other)
        {
            // TODO: Redo this method and optimize it
            int excess = 0;

            if (other._genes.Any())
            {
                excess += other.Genes.Count(gene => gene.InnovationNumber > _maxInnovation);
            }

            double weightDiff = 0;
            int disjoint = 0;
            int weightDiffCount = 0;

            foreach (ConnectionGene gene in _genes)
            {
                ConnectionGene otherGene = other.Genes.SingleOrDefault(gen => gen.InnovationNumber == gene.InnovationNumber);
                if (otherGene != default)
                {
                    weightDiff += Math.Abs(gene.Weight - otherGene.Weight);
                    weightDiffCount++;
                }
                else
                {
                    disjoint++;
                }
            }

            weightDiff = weightDiffCount == 0 ? 0 : weightDiff / weightDiffCount;

            int genomeCount = Math.Max(_genes.Count, other.Genes.Count);
            if (genomeCount < 20)
                genomeCount = 1;

            return ((TrainingRoom.TrainingRoomSettings.C1 * excess) / genomeCount +
                    (TrainingRoom.TrainingRoomSettings.C2 * disjoint) / genomeCount +
                    TrainingRoom.TrainingRoomSettings.C3 * weightDiff) <
                   TrainingRoom.TrainingRoomSettings.Threshold;
        }

        /// <summary>
        /// Check if an object is the same as this brain.
        /// An object is the same if it is a brain, and if the Brain specific equals method returns true
        /// </summary>
        /// <param name="obj">The object to compare to</param>
        /// <returns>true if it is the same</returns>
        public override bool Equals(object obj)
        {
            return obj is Brain brain && Equals(brain);
        }

        /// <summary>
        /// Check if a brain is the same as this brain
        /// 2 brains are the same if all of their genes match, their outputNodes match, their inputNodes match and the input and output counts match
        /// </summary>
        /// <param name="other">The brain to compare against</param>
        /// <returns>true if the brain is the same</returns>
        private bool Equals(Brain other)
        {
            return Genes.SequenceEqual(other.Genes) &&
                   _outputNodes.SequenceEqual(other._outputNodes) &&
                   _inputNodes.SequenceEqual(other._inputNodes) &&
                   _inputCount == other._inputCount &&
                   _outputCount == other._outputCount;
        }
    }
}
