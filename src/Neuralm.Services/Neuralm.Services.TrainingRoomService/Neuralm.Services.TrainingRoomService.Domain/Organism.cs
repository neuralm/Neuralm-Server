using Neuralm.Services.Common.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Neuralm.Services.TrainingRoomService.Domain
{
    /// <summary>
    /// Represents the <see cref="Organism"/> class.
    /// </summary>
    [DebuggerDisplay("{Generation}")]
    public class Organism : IEntity
    {
        private static readonly string[] Vowels = { "a", "e", "i", "o", "u", "y", "aa", "ee", "ie", "oo", "ou", "au" };
        private static readonly string[] Consonants = { "b", "c", "f", "g", "h", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "v", "w", "x", "z" };
        private readonly List<ConnectionGene> _childGenes = new List<ConnectionGene>();
        protected List<Node> TempNodes { get; set; } = new List<Node>();
        protected uint _maxInnovation = 0;

        protected List<HiddenNode> HiddenNodes { get; set; } = new List<HiddenNode>();

        /// <summary>
        /// Gets and sets the id.
        /// </summary>
        public Guid Id { get; protected set; }

        /// <summary>
        /// Gets and sets the species id.
        /// </summary>
        public Guid SpeciesId { get; set; }

        /// <summary>
        /// Gets and sets the list of inputs.
        /// </summary>
        public virtual List<OrganismInputNode> Inputs { get; set; }

        /// <summary>
        /// Gets and sets the list of outputs.
        /// </summary>
        public virtual List<OrganismOutputNode> Outputs { get; set; }

        /// <summary>
        /// Gets and sets the list of connection genes.
        /// </summary>
        public virtual List<ConnectionGene> ConnectionGenes { get; set; }

        /// <summary>
        /// Gets and sets the score.
        /// </summary>
        public double Score { get; set; }

        /// <summary>
        /// Gets and sets the name.
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Gets and sets the generation.
        /// </summary>
        public uint Generation { get; set; }

        /// <summary>
        /// Gets and sets a value whether the organism is leased.
        /// </summary>
        public bool IsLeased { get; set; }

        /// <summary>
        /// Gets and sets a value whether the organism is evaluated.
        /// </summary>
        public bool IsEvaluated { get; set; }

        /// <summary>
        /// EFCore entity constructor IGNORE!
        /// </summary>
        public Organism()
        {
            Inputs = new List<OrganismInputNode>();
            Outputs = new List<OrganismOutputNode>();
            ConnectionGenes = new List<ConnectionGene>();
        }

        /// <summary>
        /// Initializes an instance of the <see cref="Organism"/> class.
        /// With an initial generation of 0.
        /// </summary>
        /// <param name="trainingRoomSettings">The training room settings.</param>
        /// <param name="innovationFunction">The innovation function.</param>
        public Organism(TrainingRoomSettings trainingRoomSettings, Func<uint, uint, uint> innovationFunction) : this(0, trainingRoomSettings)
        {
            // if the generation is 0, add initial mutations.
            AddConnectionMutation(trainingRoomSettings, innovationFunction);
        }
        
        /// <summary>
        /// Initializes an instance of the <see cref="Organism"/> class.
        /// </summary>
        /// <param name="generation">The current generation.</param>
        /// <param name="trainingRoomSettings">The training room settings.</param>
        public Organism(uint generation, TrainingRoomSettings trainingRoomSettings)
        {
            // Generates a new guid for the organism, this is needed so EF can set the
            // foreign shadow key: OrganismId, on OrganismInputNode/OrganismOutputNode.
            Id = Guid.NewGuid();

            // Sets the current generation.
            Generation = generation;

            // Initializes the list of connection genes.
            ConnectionGenes = new List<ConnectionGene>();

            // Initializes the Input & Output lists for the many to many relations.
            GenerateInputAndOutputNodes(trainingRoomSettings);

            // Generates a random name for the organism.
            Name = GenerateName(trainingRoomSettings.Random.Next);
        }

        /// <summary>
        /// Initializes an instance of the <see cref="Organism"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="trainingRoomSettings">The training room settings.</param>
        /// <param name="generation">The current generation.</param>
        /// <param name="connectionGenes">The connection genes.</param>
        public Organism(Guid id, TrainingRoomSettings trainingRoomSettings, uint generation, List<ConnectionGene> connectionGenes)
        {
            Id = id;
            Generation = generation;
            ConnectionGenes = new List<ConnectionGene>(connectionGenes);
            if (connectionGenes.Any())
                _maxInnovation = ConnectionGenes.Max(p => p.InnovationNumber);
            GenerateInputAndOutputNodes(trainingRoomSettings);
            Name = GenerateName(trainingRoomSettings.Random.Next);
        }

        /// <summary>
        /// Checks if an object is the same as this organism.
        /// An object is the same if it is a organism, and if the organism specific equals method returns true.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <param name="once">The value that determines whether the inputs and outputs should also be used in the equals equation.</param>
        /// <returns>Returns <c>true</c> if it is the same; otherwise, <c>false</c>.</returns>
        public virtual bool Equals(object obj, bool once = true)
        {
            if (!(obj is Organism organism))
                return false;

            // Checks if a organism is the same as this organism.
            // 2 organisms are the same if all of their genes match, their outputNodes match,
            // their inputNodes match and the input and output counts match.
            bool sequenceEqual = ConnectionGenes.SequenceEqual(organism.ConnectionGenes);
            if (once)
                return sequenceEqual;

            bool equal = Inputs.SequenceEqual(organism.Inputs);
            bool b = Outputs.SequenceEqual(organism.Outputs);
            return sequenceEqual &&
                   equal &&
                   b;
        }

        /// <summary>
        /// Checks if the other organism is of the same species.
        /// </summary>
        /// <param name="organism">The other organism.</param>
        /// <param name="trainingRoomSettings">The training room settings.</param>
        /// <returns>Returns <c>true</c> if the other organism is of the same species; otherwise, <c>false</c>.</returns>
        public bool IsSameSpecies(Organism organism, TrainingRoomSettings trainingRoomSettings)
        {
            //TODO: Check inputs and outputs when they mutate
            // Sets the initial excess.
            int excess = 0;

            // If the organism has connection genes then add the amount of connection genes 
            // where the innovation number is higher than the max innovation to the excess.
            if (organism.ConnectionGenes.Any())
                excess += organism.ConnectionGenes.Count(gene => gene.InnovationNumber > _maxInnovation);

            // Sets initial variables.
            double weightDiff = 0;
            int disjoint = 0;
            int weightDiffCount = 0;

            // For each connection gene where the given organism contains a single connection gene with the same innovation number
            // add the weight difference to the weight difference variable and add 1 to the weight difference counter; otherwise, add one 
            // to the disjoint counter.
            foreach (ConnectionGene gene in ConnectionGenes)
            {
                ConnectionGene otherGene = organism.ConnectionGenes.SingleOrDefault(gen => gen.InnovationNumber == gene.InnovationNumber);
                if (otherGene != default)
                {
                    weightDiff += Math.Abs(gene.Weight - otherGene.Weight);
                    weightDiffCount++;
                }
                else
                    disjoint++;
            }

            // If the weight difference count is not 0 than compound weight difference to weight difference count.
            if (weightDiffCount != 0)
                weightDiff /= weightDiffCount;

            // Set genome count to the max between connection genes count of the current organism and the other organism connection genes count.
            int genomeCount = Math.Max(ConnectionGenes.Count, organism.ConnectionGenes.Count);

            // The coefficients SpeciesExcessGeneWeight, SpeciesDisjointGeneWeight, and SpeciesAverageWeightDiffWeight
            // allow us to adjust the importance of the three factors, and the factor N,
            // the number of genes in the larger genome, normalizes for genome size
            // (N can be set to 1 if both genomes are small, i.e., consist of fewer than 20 genes).
            if (genomeCount < 20)
                genomeCount = 1;

            // Depending on the training room settings return whether the organism is of the same species.
            return ((trainingRoomSettings.SpeciesExcessGeneWeight * excess) / genomeCount +
                    (trainingRoomSettings.SpeciesDisjointGeneWeight * disjoint) / genomeCount +
                    trainingRoomSettings.SpeciesAverageWeightDiffWeight * weightDiff) <
                   trainingRoomSettings.Threshold;
        }

        /// <summary>
        /// Creates an organism based on the genes of 2 parent organisms. 
        /// For each gene in parent1 we check if parent2 has a gene with the same innovation number, if this is the case we randomly choose a parent to get the gene from.
        /// If a gene only exists in one parent we add it no matter what.
        /// </summary>
        /// <param name="parent2Organism">The other parent.</param>
        /// <param name="trainingRoomSettings">The training room settings.</param>
        /// <returns>Returns a child organism based on the genes of this organism and the passed organism.</returns>
        public Organism Crossover(Organism parent2Organism, TrainingRoomSettings trainingRoomSettings, Func<Guid, TrainingRoomSettings, uint, List<ConnectionGene>, Organism> constructOrganism)
        {
            // Pre-generates a child id for creating connection genes.
            Guid childId = Guid.NewGuid();

            // Copies the organisms from parent2.
            List<ConnectionGene> parent2 = parent2Organism.ConnectionGenes.Select(gene => gene.Clone(childId)).ToList();

            foreach (ConnectionGene gene in ConnectionGenes)
            {
                // Prepares a gene to remove and add.
                ConnectionGene geneToRemove = default;

                // Clone the gene to add with the new id.
                ConnectionGene geneToAdd = gene.Clone(childId);
                
                // Tries to find a connection gene based on the current connection gene in the parent, and if not found, gives a default.
                ConnectionGene gene2 = parent2.SingleOrDefault(gen => gen.InnovationNumber == gene.InnovationNumber);

                // If the gene is not a default connection gene.
                if (gene2 != default)
                {
                    // Set it for removal.
                    geneToRemove = gene2;

                    // Depending on a random set the current gene or a random gene to add.
                    if (trainingRoomSettings.Random.NextDouble() > 0.5)
                        geneToAdd = gene2.Clone(childId);
                }

                // If the gene to remove is not default, remove it from its parent.
                if (geneToRemove != default)
                    parent2.Remove(geneToRemove);

                // If the gene to add is not enabled and a random is lower than the enable connection chance, enable the gene.
                if (!geneToAdd.Enabled && trainingRoomSettings.Random.NextDouble() < trainingRoomSettings.EnableConnectionChance)
                    geneToAdd.Enabled = true;

                // Add gene to the temporary child genes list.
                _childGenes.Add(geneToAdd);
            }

            // Add all remaining genes in the parent to the child genes.
            foreach (ConnectionGene geneToAdd in parent2)
            {
                //Make sure this gene doesn't cause a loop
                if (!ConnectionLoops(geneToAdd.InNodeIdentifier, geneToAdd.OutNodeIdentifier, _childGenes))
                {
                    _childGenes.Add(geneToAdd);
                }
            }

            // Create a new organism with all new genes.
            Organism organism = constructOrganism(childId, trainingRoomSettings, Generation + 1, _childGenes);

            // Clears the temporary child genes list for re-use.
            _childGenes.Clear();

            // Return the newly created organism.
            return organism;
        }

        /// <summary>
        /// Mutates the the organism based on the training room settings.
        /// </summary>
        /// <param name="trainingRoomSettings">The training room settings.</param>
        /// <param name="getAndIncreaseNodeIdFunction">The get and increase node id function.</param>
        /// <param name="innovationFunction">The innovation function.</param>
        public void Mutate(TrainingRoomSettings trainingRoomSettings, Func<uint> getAndIncreaseNodeIdFunction, Func<uint, uint, uint> innovationFunction)
        {
            // If the random value is lower than the add connection chance add a connection mutation.
            if (trainingRoomSettings.Random.NextDouble() < trainingRoomSettings.AddConnectionChance)
                AddConnectionMutation(trainingRoomSettings, innovationFunction);

            // If the random value is lower than the add node chance add a node mutation.
            if (trainingRoomSettings.Random.NextDouble() < trainingRoomSettings.AddNodeChance)
                AddNodeMutation(trainingRoomSettings, getAndIncreaseNodeIdFunction, innovationFunction);

            // If the random value is higher than the mutate weight chance or the connection gene count is lower than 0 return early.
            if (trainingRoomSettings.Random.NextDouble() > trainingRoomSettings.MutateWeightChance || ConnectionGenes.Count <= 0)
                return;

            // Find a random connection gene.
            ConnectionGene connectionGene = ConnectionGenes[trainingRoomSettings.Random.Next(ConnectionGenes.Count)];

            // If the random value is lower than the weight reassign chance set a new weight else add weight.
            if (trainingRoomSettings.Random.NextDouble() < trainingRoomSettings.WeightReassignChance)
                connectionGene.Weight = trainingRoomSettings.Random.NextDouble() * 2 - 1;
            else
                connectionGene.Weight += (trainingRoomSettings.Random.NextDouble() * 2 - 1) * 0.1;

            //TODO: Inputs mutate?
        }

        /// <summary>
        /// Clones the organism.
        /// </summary>
        /// <param name="trainingRoomSettings">The training room settings.</param>
        /// <returns>Returns a clone of the organism.</returns>
        public virtual Organism Clone(TrainingRoomSettings trainingRoomSettings)
        {
            // Prepares a new id for the organisms to clone with.
            Guid newGuid = Guid.NewGuid();

            // Create a new organism with the given node id and training room settings.
            return new Organism(newGuid, trainingRoomSettings, Generation, ConnectionGenes.Select(gene => gene.Clone(newGuid)).ToList());
            //TODO: When inputs can mutate they should also be cloned
        }

        /// <summary>
        /// Adds a node by selecting a random connection, disabling it and replacing it by 2 genes.
        /// The first gene goes from the original input node, to the new node with a weight of 1.
        /// The second gene goes from the the new node to the old output node with the same weight as the original connection.
        /// </summary>
        /// <param name="trainingRoomSettings">The training room settings.</param>
        /// <param name="getAndIncreaseNodeIdFunction">The get and increase node id function.</param>
        /// <param name="innovationFunction">The innovation function.</param>
        private void AddNodeMutation(TrainingRoomSettings trainingRoomSettings, Func<uint> getAndIncreaseNodeIdFunction, Func<uint, uint, uint> innovationFunction)
        {
            // If the connection genes count is 0, then there are not connection genes to mutate.
            if (ConnectionGenes.Count == 0)
                return;

            // Find a random connection.
            ConnectionGene theChosenOne = ConnectionGenes.ElementAt(trainingRoomSettings.Random.Next(ConnectionGenes.Count));

            // Disable the connection gene.
            theChosenOne.Enabled = false;

            // Generate the new node's id.
            uint newNodeId = getAndIncreaseNodeIdFunction();

            // Get the old in and out node identifiers.
            uint oldInId = theChosenOne.InNodeIdentifier;
            uint oldOutId = theChosenOne.OutNodeIdentifier;

            // Create a connectionGene from oldIn (a) to new (c) and from new (c) to oldOut (b) 
            ConnectionGene aToC = CreateConnectionGene(innovationFunction(oldInId, newNodeId), oldInId, newNodeId, 1);
            ConnectionGene cToB = CreateConnectionGene(innovationFunction(newNodeId, oldOutId), newNodeId, oldOutId, theChosenOne.Weight);

            // Add new connection genes.
            ConnectionGenes.Add(aToC);
            ConnectionGenes.Add(cToB);

            // Update the max innovation.
            _maxInnovation = Math.Max(Math.Max(cToB.InnovationNumber, aToC.InnovationNumber), _maxInnovation);
        }

        /// <summary>
        /// Adds a new connection between 2 randomly chosen nodes.
        /// The first node can not be an output.
        /// The second node cannot be an input node if the first node is an input node, and the layer cannot be the same as the first node.
        /// 
        /// If the connection that is going to be created already exists, there will be a maximum of 5 attempts to create a new one.
        /// </summary>
        /// <param name="trainingRoomSettings">The training room settings.</param>
        /// <param name="innovationFunction">The innovation function.</param>
        protected void AddConnectionMutation(TrainingRoomSettings trainingRoomSettings, Func<uint, uint, uint> innovationFunction)
        {
            // Set initial attempt counter to 0.
            int attemptsDone = 0;

            // Rebuild structure so we get layer information to make sure we don't get circular dependencies.
            RebuildStructure();

            while (true)
            {
                // Prepare start and end node.
                Node startNode;
                Node endNode;

                // Set start node to a random node while start note is of type OutputNode.
                do
                {
                    startNode = TempNodes[trainingRoomSettings.Random.Next(TempNodes.Count)];
                } while (IsNodeAnOutputNode(startNode));


                // Set end node to a random node while the layer of the start and end node are the same or
                // start node is of type InputNode and end node is of type InputNode.
                do
                {
                    endNode = TempNodes[trainingRoomSettings.Random.Next(TempNodes.Count)];
                } while (endNode.Layer == startNode.Layer || (IsNodeAnInputNode(startNode) && IsNodeAnInputNode(endNode)));

                // If end node layer is higher than start node layer swap them.
                if (endNode.Layer > startNode.Layer)
                {
                    Node temp = endNode;
                    endNode = startNode;
                    startNode = temp;
                }

                // If the connection between the selected start and end node already exists continue the loop, until 5 attempts are done,
                // then return early.
                if (ConnectionExists(startNode, endNode))
                {
                    if (attemptsDone >= 5)
                        return;
                    attemptsDone += 1;
                    continue;
                }
                // Create a new connection gene from the start and end nodes.
                ConnectionGene connection = CreateConnectionGene(innovationFunction(startNode.NodeIdentifier, endNode.NodeIdentifier), startNode.NodeIdentifier, endNode.NodeIdentifier, trainingRoomSettings.Random.NextDouble() * 2 - 1);

                // Add the connection gene.
                ConnectionGenes.Add(connection);

                // Update the max innovation.
                _maxInnovation = Math.Max(connection.InnovationNumber, _maxInnovation);

                // Break the loop.
                break;
            }
        }

        protected virtual ConnectionGene CreateConnectionGene(uint innovationNumber, uint inNodeId, uint outNodeId, double weight, bool enabled = true)
        {
            // Create a new connection gene from the start and end nodes.
            ConnectionGene connection = new ConnectionGene(Id, innovationNumber, inNodeId, outNodeId, weight, enabled);
            return connection;
        }

        protected virtual bool IsNodeAnInputNode(Node node)
        {
            return node is InputNode;
        }

        protected virtual bool IsNodeAnOutputNode(Node node)
        {
            return node is OutputNode;
        }

        /// <summary>
        /// Check if adding a connection between the given nodes causes a loop in the given genes.
        /// </summary>
        /// <param name="startNodeIdentifier">The new connection's start node.</param>
        /// <param name="endNodeIdentifier">The new connection's end node.</param>
        /// <param name="connectionGenes">The already existing genes.</param>
        /// <returns>Returns <c>true</c> if adding a connection between the given nodes causes a loop in the given genes; otherwise, <c>false</c>.</returns>
        private static bool ConnectionLoops(uint startNodeIdentifier, uint endNodeIdentifier, List<ConnectionGene> connectionGenes)
        {
            return connectionGenes
                .FindAll(gene => gene.OutNodeIdentifier == startNodeIdentifier)
                .Any(gene => gene.InNodeIdentifier == endNodeIdentifier 
                             || ConnectionLoops(gene.InNodeIdentifier, endNodeIdentifier, connectionGenes));
        }

        /// <summary>
        /// Rebuild structure so we get layer information to make sure there are not circular dependencies.
        /// </summary>
        private void RebuildStructure()
        {
            // Clear the temporary node storage.
            TempNodes.Clear();

            // Load all nodes for each connection gene.
            ConnectionGenes.ForEach(LoadNodes);

            // Find all the node in the connection genes.
            TempNodes.AddRange(ConnectionGenes.SelectMany(p => new[] { p.InNode, p.OutNode }).Distinct());

            // Add all input nodes
            TempNodes.AddRange(Inputs.Select(p => p.InputNode));

            // Add all output nodes.
            TempNodes.AddRange(Outputs.Select(p => p.OutputNode));

            //Set all nodes to the lowest value so they dont keep older values.
            foreach (Node node in TempNodes)
            {
                if (!IsNodeAnOutputNode(node))
                {
                    SetLayer(node, uint.MinValue, true);
                }
            }

            foreach (Node node in TempNodes)
            {
                if (IsNodeAnOutputNode(node))
                {
                    // For each node of type OutputNode set the layer to 0
                    SetLayer(node, 0);
                }
                else if (IsNodeAnInputNode(node))
                {
                    // For each node of type InputNode set the layer to minimum value and force to true.
                    SetLayer(node, uint.MaxValue, true);
                }
            }
        }

        protected virtual void LoadNodes(ConnectionGene connectionGene)
        {
            // Get or create the In and Out nodes based on the given node identifiers.
            connectionGene.InNode = GetNodeFromIdentifier(connectionGene.InNodeIdentifier);
            connectionGene.OutNode = GetNodeFromIdentifier(connectionGene.OutNodeIdentifier);

            //Debug.Assert(!connectionGene.OutNode.GetType().IsAssignableFrom(typeof(InputNode)), "Output node of connection gene is an InputNode.");
        }

        private void SetLayer(Node node, uint layer, bool force = false)
        {
            // Set the layer depending on force and if the current layer is higher than the layer given.
            if (force)
            {
                node.Layer = layer;
                // If force is true, return early.
                return;
            }

            node.Layer = Math.Max(layer, node.Layer);

            // For each connection gene where the out node identifier is the same as the given node identifier
            // set the layer to the node's layer plus one.
            foreach (ConnectionGene connectionGene in ConnectionGenes)
            {
                if (connectionGene.OutNodeIdentifier.Equals(node.NodeIdentifier))
                {
                    SetLayer(connectionGene.InNode, node.Layer + 1);
                }
            }
        }

        public Node GetNodeFromIdentifier(uint nodeIdentifier)
        {
            // Tries to find the first relation where the input node has the given identifier or return default.
            OrganismInputNode organismInputNode = Inputs.FirstOrDefault(n => n.InputNode.NodeIdentifier == nodeIdentifier);

            // if the organism input node is not default return the input node.
            if (organismInputNode != default) 
                return organismInputNode.InputNode;

            // Tries to find the first relation where the output node has the given identifier, else return default.
            OutputNode nodeFromIdentifier = Outputs.FirstOrDefault(n => n.OutputNode.NodeIdentifier == nodeIdentifier)?.OutputNode;
            return nodeFromIdentifier ?? CreateAndAddNode(nodeIdentifier);
        }

        protected virtual Node CreateAndAddNode(uint nodeIdentifier)
        {
            // Tries to find the node in the hidden nodes list, if not found returns a default.
            HiddenNode node = HiddenNodes.FirstOrDefault(n => n.NodeIdentifier == nodeIdentifier);

            // If the node is not equal to default return the given node.
            if (node != default)
                return node;

            // If no node was found then create a hidden node.
            node = new HiddenNode(nodeIdentifier);

            // Add it to the hidden nodes list.
            HiddenNodes.Add(node);

            // Return the hidden node.
            return node;
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
            return ConnectionGenes.Any(gene => gene.InNode.Id.Equals(startNode.Id) && gene.OutNode.Id.Equals(endNode.Id));
        }

        /// <summary>
        /// Generates the input and output nodes.
        /// </summary>
        /// <param name="trainingRoomSettings">The training room settings.</param>
        protected virtual void GenerateInputAndOutputNodes(TrainingRoomSettings trainingRoomSettings)
        {
            Inputs = new List<OrganismInputNode>((int)trainingRoomSettings.InputCount);
            for (uint nodeIdentifier = 0; nodeIdentifier < trainingRoomSettings.InputCount; nodeIdentifier++)
            {
                Inputs.Add(new OrganismInputNode(this, new InputNode(nodeIdentifier)));
            }
            Outputs = new List<OrganismOutputNode>((int)trainingRoomSettings.OutputCount);
            for (uint nodeIdentifier = 0; nodeIdentifier < trainingRoomSettings.OutputCount; nodeIdentifier++)
            {
                Outputs.Add(new OrganismOutputNode(this, new OutputNode(nodeIdentifier + trainingRoomSettings.InputCount)));
            }
        }

        /// <summary>
        /// Generates a random name.
        /// </summary>
        /// <param name="randomNext">The function that generates a random number between 0 and x.</param>
        /// <returns>Returns a random generated name.</returns>
        protected static string GenerateName(Func<int, int> randomNext)
        {
            string name = Consonants[randomNext(Consonants.Length)];

            name += Vowels[randomNext(Vowels.Length)];

            return randomNext(name.Length) == 0
                ? name + GenerateName(randomNext)
                : name + Consonants[randomNext(Consonants.Length)];
        }

        public override string ToString()
        {
            return
                $"Id: {Id}, SpeciesId: {SpeciesId}, Generation: {Generation}, Score: {Score}, Inputs: {Inputs.Count}, " +
                $" Outputs: {Outputs.Count}, ConnectionGenes: {ConnectionGenes.Count}, Name: {Name}, " +
                $"IsLeased: {IsLeased}, IsEvaluated: {IsEvaluated}";
        }
    }
}
