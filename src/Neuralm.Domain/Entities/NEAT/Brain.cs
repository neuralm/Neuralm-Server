using System;
using System.Collections.Generic;
using System.Linq;

namespace Neuralm.Domain.Entities.NEAT
{
    /// <summary>
    /// Represents the <see cref="Brain"/> class provides methods to crossover, mutate and add connection genes.
    /// </summary>
    public class Brain
    {
        private readonly Dictionary<uint, Node> _nodes = new Dictionary<uint, Node>();
        private readonly List<Node> _outputNodes = new List<Node>();
        private readonly List<Node> _inputNodes = new List<Node>();
        private readonly List<ConnectionGene> _childGenes = new List<ConnectionGene>();
        private uint _maxInnovation;

        /// <summary>
        /// Gets the list of genes.
        /// </summary>
        public virtual List<ConnectionGene> ConnectionGenes { get; private set; } = new List<ConnectionGene>();

        /// <summary>
        /// Gets and sets the id.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets and sets the training room id.
        /// </summary>
        public Guid TrainingRoomId { get; private set; }

        /// <summary>
        /// Gets and sets the organism id.
        /// </summary>
        public Guid OrganismId { get; set; }

        /// <summary>
        /// Gets and sets the training room.
        /// </summary>
        public virtual TrainingRoom TrainingRoom { get; private set; }

        /// <summary>
        /// EFCore entity constructor IGNORE!
        /// </summary>
        protected Brain()
        {
            
        }

        /// <summary>
        /// Initializes an instance of the <see cref="Brain"/> class with a set amount of input and output nodes.
        /// </summary>
        /// <param name="trainingRoom">The training room this brain is a part of.</param>
        public Brain(TrainingRoom trainingRoom)
        {
            TrainingRoom = trainingRoom;
            TrainingRoomId = TrainingRoom.Id;

            if (Id.Equals(Guid.Empty))
                Id = Guid.NewGuid();

            for (uint i = 0; i < TrainingRoom.TrainingRoomSettings.InputCount; i++)
            {
                Node node = new Node(i, NodeType.Input);
                _inputNodes.Add(node);
                _nodes[i] = node;
            }

            for (uint i = 0; i < TrainingRoom.TrainingRoomSettings.OutputCount; i++)
            {
                Node node = new Node(TrainingRoom.TrainingRoomSettings.InputCount + i, NodeType.Output);
                _outputNodes.Add(node);
                _nodes[TrainingRoom.TrainingRoomSettings.InputCount + i] = node;
            }

            trainingRoom.IncreaseNodeIdTo(TrainingRoom.TrainingRoomSettings.InputCount + TrainingRoom.TrainingRoomSettings.OutputCount);
        }

        /// <summary>
        /// Initializes an instance of the <see cref="Brain"/> class with a set amount of input and output nodes and provided genes.
        /// </summary>
        /// <param name="id">The id for the brain.</param>
        /// <param name="trainingRoom">The training room this brain is a part of.</param>
        /// <param name="genes">The genes to create the brain out of.</param>
        public Brain(Guid id, TrainingRoom trainingRoom, IEnumerable<ConnectionGene> genes) : this(trainingRoom)
        {
            Id = id;
            foreach (ConnectionGene gene in genes)
            {
                ConnectionGenes.Add(gene.Clone());
                _maxInnovation = Math.Max(gene.InnovationNumber, _maxInnovation);
            }
        }

        /// <summary>
        /// Creates a brain based on the genes of 2 parent brains. 
        /// For each gene in parent1 we check if parent2 has a gene with the same innovation number, if this is the case we randomly choose a parent to get the gene from.
        /// If a gene only exists in one parent we add it no matter what.
        /// </summary>
        /// <param name="parent2Brain">The other parent.</param>
        /// <returns>Returns a child brain based on the genes of this brain and the passed brain.</returns>
        public Brain Crossover(Brain parent2Brain)
        {
            _childGenes.Clear();
            List<ConnectionGene> parent2 = new List<ConnectionGene>(parent2Brain.ConnectionGenes);

            foreach (ConnectionGene gene in ConnectionGenes)
            {
                ConnectionGene geneToRemove = default;
                ConnectionGene geneToAdd = gene;

                ConnectionGene gene2 = parent2Brain.ConnectionGenes.SingleOrDefault(gen => gen.InnovationNumber == gene.InnovationNumber);
                if (gene2 != default)
                {
                    geneToRemove = gene2;
                    geneToAdd = TrainingRoom.Random.NextDouble() < 0.5 ? gene : gene2;
                }

                geneToAdd = geneToAdd.Clone();

                if (geneToRemove != default)
                    parent2.Remove(geneToRemove);

                if (!geneToAdd.Enabled && TrainingRoom.Random.NextDouble() < TrainingRoom.TrainingRoomSettings.EnableConnectionChance)
                {
                    geneToAdd.Enabled = true;
                }

                _childGenes.Add(geneToAdd);
            }

            _childGenes.AddRange(parent2);
            return new Brain(Guid.NewGuid(), TrainingRoom, _childGenes);
        }

        /// <summary>
        /// Get the node with the given id, or create it if there is no node with this id.
        /// </summary>
        /// <param name="id">The node's ID</param>
        /// <returns>Returns the node.</returns>
        public Node GetOrCreateNode(uint id)
        {
            return !_nodes.ContainsKey(id) ? _nodes[id] = new Node(id, NodeType.Hidden) : _nodes[id];
        }

        /// <summary>
        /// Clones this brain to produce a brain that equals the other brain but is not the same instance.
        /// </summary>
        /// <param name="newId">Determines whether a new Id should be generated for the clone.</param>
        /// <returns>Returns a new brain with the same genes, training room, inputCount and outputCount.</returns>
        public Brain Clone(bool newId = false)
        {
            return new Brain(newId ? Guid.NewGuid() : Id, TrainingRoom, ConnectionGenes.Select(gene => gene.Clone()).ToList());
        }

        /// <summary>
        /// Rebuilds the brains structure based on the genes.
        /// This will add nodes, create the dependencies and set the layers.
        /// </summary>
        private void RebuildStructure()
        {
            foreach (ConnectionGene gene in ConnectionGenes)
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
        /// Mutates the brain, each time this is called there is a chance to:
        /// - Add a connection
        /// - Add a node
        /// - Change the weight of an existing gene, which can either randomly be reset to a random value or slightly change
        /// These changes can all happen in one call, but the chance that this happens depends on the training room settings.
        /// </summary>
        public void Mutate()
        {
            if (TrainingRoom.Random.NextDouble() < TrainingRoom.TrainingRoomSettings.AddConnectionChance)
                AddConnectionMutation();

            if (TrainingRoom.Random.NextDouble() < TrainingRoom.TrainingRoomSettings.AddNodeChance)
                AddNodeMutation();

            if (!(TrainingRoom.Random.NextDouble() < TrainingRoom.TrainingRoomSettings.MutateWeightChance) || ConnectionGenes.Count <= 0)
                return;
            ConnectionGene connectionGene = ConnectionGenes.ElementAt(TrainingRoom.Random.Next(ConnectionGenes.Count));
            if (TrainingRoom.Random.NextDouble() < TrainingRoom.TrainingRoomSettings.WeightReassignChance)
                connectionGene.Weight = TrainingRoom.Random.NextDouble() * 2 - 1;
            else
                connectionGene.Weight += (TrainingRoom.Random.NextDouble() * 2 - 1) * 0.1;
        }

        /// <summary>
        /// Adds a new connection between 2 randomly chosen nodes.
        /// The first node can not be an output.
        /// The second node cannot be an input node if the first node is an input node, and the layer cannot be the same as the first node.
        /// 
        /// If the connection that is going to be created already exists, there will be a maximum of 5 attempts to create a new one.
        /// </summary>
        private void AddConnectionMutation()
        {
            int attemptsDone = 0;
            while (true)
            {
                // NOTE: Rebuild structure so we get layer information to make sure we don't get circle dependencies
                RebuildStructure(); 

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
                ConnectionGenes.Add(connection);
                _maxInnovation = Math.Max(connection.InnovationNumber, _maxInnovation);
                break;
            }
        }

        /// <summary>
        /// Checks if a connection between 2 nodes already exists.
        /// This check is directional, this means that ConnectionExists(a,b) is not the same as ConnectionExists(b,a).
        /// </summary>
        /// <param name="startNode">The start node.</param>
        /// <param name="endNode">The end node.</param>
        /// <returns>Returns <c>true</c> if there is a connection that goes from the start node to the end node; otherwise, <c>false</c>.</returns>
        private bool ConnectionExists(Node startNode, Node endNode)
        {
            return ConnectionGenes.Any(gene => gene.InId == startNode.Id && gene.OutId == endNode.Id);
        }

        /// <summary>
        /// Adds a node by selecting a random connection, disabling it and replacing it by 2 genes.
        /// The first gene goes from the original input node, to the new node with a weight of 1.
        /// The second gene goes from the the new node to the old output node with the same weight as the original connection.
        /// </summary>
        private void AddNodeMutation()
        {
            if (ConnectionGenes.Count == 0)
                return;

            // Choose a random connection to add a node into
            ConnectionGene theChosenOne = ConnectionGenes.ElementAt(TrainingRoom.Random.Next(ConnectionGenes.Count));

            // Disable it
            theChosenOne.Enabled = false;

            // Generate the new node's id
            uint newNodeId = TrainingRoom.GetAndIncreaseNodeId();

            uint oldInId = theChosenOne.InId;
            uint oldOutId = theChosenOne.OutId;

            // Create a connectionGene from oldIn (a) to new (c) and from new (c) to oldOut (b) 
            ConnectionGene aToC = new ConnectionGene(Id, oldInId, newNodeId, 1, TrainingRoom.GetInnovationNumber(oldInId, newNodeId));
            ConnectionGene cToB = new ConnectionGene(Id, newNodeId, oldOutId, theChosenOne.Weight, TrainingRoom.GetInnovationNumber(newNodeId, oldOutId));

            ConnectionGenes.Add(aToC);
            ConnectionGenes.Add(cToB);
            _maxInnovation = Math.Max(Math.Max(cToB.InnovationNumber, aToC.InnovationNumber), _maxInnovation);
        }

        /// <summary>
        /// Checks if the other brain is the same species as this brain.
        /// </summary>
        /// <param name="other">The other brain.</param>
        /// <returns>Returns <c>true</c> if the other brain is the same species as this brain; otherwise, <c>false</c>.</returns>
        public bool IsSameSpecies(Brain other)
        {
            // TODO: Redo this method and optimize it
            int excess = 0;

            if (other.ConnectionGenes.Any())
            {
                excess += other.ConnectionGenes.Count(gene => gene.InnovationNumber > _maxInnovation);
            }

            double weightDiff = 0;
            int disjoint = 0;
            int weightDiffCount = 0;

            foreach (ConnectionGene gene in ConnectionGenes)
            {
                ConnectionGene otherGene = other.ConnectionGenes.SingleOrDefault(gen => gen.InnovationNumber == gene.InnovationNumber);
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

            int genomeCount = Math.Max(ConnectionGenes.Count, other.ConnectionGenes.Count);
            if (genomeCount < 20)
                genomeCount = 1;

            return ((TrainingRoom.TrainingRoomSettings.SpeciesExcessGeneWeight * excess) / genomeCount +
                    (TrainingRoom.TrainingRoomSettings.SpeciesDisjointGeneWeight * disjoint) / genomeCount +
                    TrainingRoom.TrainingRoomSettings.SpeciesAverageWeightDiffWeight * weightDiff) <
                   TrainingRoom.TrainingRoomSettings.Threshold;
        }

        /// <summary>
        /// Checks if an object is the same as this brain.
        /// An object is the same if it is a brain, and if the Brain specific equals method returns true.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns>Returns <c>true</c> if it is the same; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return obj is Brain brain && Equals(brain);
        }

        /// <summary>
        /// Checks if a brain is the same as this brain.
        /// 2 brains are the same if all of their genes match, their outputNodes match, their inputNodes match and the input and output counts match.
        /// </summary>
        /// <param name="other">The brain to compare against.</param>
        /// <returns>Returns <c>true</c> if the brain is the same; otherwise, <c>false</c>.</returns>
        private bool Equals(Brain other)
        {
            return ConnectionGenes.SequenceEqual(other.ConnectionGenes) &&
                   _outputNodes.SequenceEqual(other._outputNodes) &&
                   _inputNodes.SequenceEqual(other._inputNodes) &&
                   TrainingRoom?.TrainingRoomSettings?.InputCount == other.TrainingRoom?.TrainingRoomSettings?.InputCount &&
                   TrainingRoom?.TrainingRoomSettings?.OutputCount == other.TrainingRoom?.TrainingRoomSettings?.OutputCount;
        }
    }
}
