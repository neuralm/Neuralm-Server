using Neuralm.Services.TrainingRoomService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Neuralm.Services.TrainingRoomService.Domain.Evaluatables
{
    /// <inheritdoc />
    public class EvaluatableOrganism : Organism
    {
        // public EvaluatableOrganism(uint generation, TrainingRoomSettings trainingRoomSettings)
        // {
        //     Id = Guid.NewGuid();
        //
        //     // Sets the current generation.
        //     Generation = generation;
        //
        //     // Initializes the list of connection genes.
        //     ConnectionGenes = new List<ConnectionGene>();
        //
        //     // Initializes the Input & Output lists for the many to many relations.
        //     GenerateInputAndOutputNodes(trainingRoomSettings);
        //
        //     // Generates a random name for the organism.
        //     Name = GenerateName(trainingRoomSettings.Random.Next);
        // }
        
        // public EvaluatableOrganism(TrainingRoomSettings trainingRoomSettings, Func<uint, uint, uint> innovationFunction) : 
        //     this(0, trainingRoomSettings)
        // {
        //     AddConnectionMutation(trainingRoomSettings, innovationFunction);
        // }
        
        public EvaluatableOrganism(TrainingRoomSettings trainingRoomSettings, Func<uint, uint, uint> innovationFunction, uint generation)
        {
            Id = Guid.NewGuid();

            // Sets the current generation.
            Generation = generation;

            // Initializes the list of connection genes.
            ConnectionGenes = new List<ConnectionGene>();

            // Initializes the Input & Output lists for the many to many relations.
            GenerateInputAndOutputNodes(trainingRoomSettings);

            // Generates a random name for the organism.
            Name = GenerateName(trainingRoomSettings.Random.Next);
            
            foreach (OrganismInputNode organismInputNode in Inputs)
            {
                InputNode inputNode = organismInputNode.InputNode;
                
                foreach (OrganismOutputNode organismOutputNode in Outputs)
                {
                    OutputNode outputNode = organismOutputNode.OutputNode;
                    
                    // Create a new connection gene from the start and end nodes.
                    ConnectionGene connection = CreateConnectionGene(innovationFunction(inputNode.NodeIdentifier, outputNode.NodeIdentifier), inputNode.NodeIdentifier, outputNode.NodeIdentifier, trainingRoomSettings.Random.NextDouble() * 2 - 1);

                    // Add the connection gene.
                    ConnectionGenes.Add(connection);
                }
            }
        }
        
        public EvaluatableOrganism(Guid id, TrainingRoomSettings trainingRoomSettings, uint generation, List<ConnectionGene> connectionGenes)
        {
            Id = id;
            Generation = generation;
            ConnectionGenes = new List<ConnectionGene>(connectionGenes);
            if (connectionGenes.Any())
                _maxInnovation = ConnectionGenes.Max(p => p.InnovationNumber);
            GenerateInputAndOutputNodes(trainingRoomSettings);
            Name = GenerateName(trainingRoomSettings.Random.Next);
        }

        public double[] Evaluate(double[] inputs)
        {
            if (inputs.Length != Inputs.Count)
            {
                throw new ArgumentOutOfRangeException($"Inputs length ${inputs.Length} should match input nodes length {Inputs.Count}");
            }

            for (int i = 0; i < inputs.Length; i++)
            {
                ((EvaluatableInputNode) Inputs[i].InputNode).SetValue(inputs[i]);
            }

            double[] outputs = new double[Outputs.Count];
            for (int i = 0; i < Outputs.Count; i++)
            {
                outputs[i] = ((EvaluatableOutputNode) Outputs[i].OutputNode).GetValue();
            }

            return outputs;
        }

        protected override Node CreateAndAddNode(uint nodeIdentifier)
        {
            // Tries to find the node in the hidden nodes list, if not found returns a default.
            EvaluatableHiddenNode node = (EvaluatableHiddenNode) HiddenNodes.FirstOrDefault(n => n.NodeIdentifier == nodeIdentifier);

            // If the node is not equal to default return the given node.
            if (node != default)
                return node;

            // If no node was found then create a hidden node.
            node = new EvaluatableHiddenNode(nodeIdentifier);

            // Add it to the hidden nodes list.
            HiddenNodes.Add(node);

            // Return the hidden node.
            return node;
        }

        protected override ConnectionGene CreateConnectionGene(uint innovationNumber, uint inNodeId, uint outNodeId, double weight, bool enabled = true)
        {
            EvaluatableConnectionGene connection = new EvaluatableConnectionGene(Id, innovationNumber, inNodeId, outNodeId, weight, enabled);
            // Create a new connection gene from the start and end nodes.
            return connection;
        }

        protected override bool IsNodeAnInputNode(Node node)
        {
            return node is EvaluatableInputNode;
        }

        protected override bool IsNodeAnOutputNode(Node node)
        {
            return node is EvaluatableOutputNode;
        }

        protected override void GenerateInputAndOutputNodes(TrainingRoomSettings trainingRoomSettings)
        {
            Inputs = new List<OrganismInputNode>((int)trainingRoomSettings.InputCount);
            for (uint nodeIdentifier = 0; nodeIdentifier < trainingRoomSettings.InputCount; nodeIdentifier++)
            {
                Inputs.Add(new OrganismInputNode(this, new EvaluatableInputNode(nodeIdentifier)));
            }
            Outputs = new List<OrganismOutputNode>((int)trainingRoomSettings.OutputCount);
            for (uint nodeIdentifier = 0; nodeIdentifier < trainingRoomSettings.OutputCount; nodeIdentifier++)
            {
                Outputs.Add(new OrganismOutputNode(this, new EvaluatableOutputNode(nodeIdentifier + trainingRoomSettings.InputCount)));
            }
        }

        public override Organism Clone(TrainingRoomSettings trainingRoomSettings)
        {
            // Prepares a new id for the organisms to clone with.
            Guid newGuid = Guid.NewGuid();

            // Create a new organism with the given node id and training room settings.
            return new EvaluatableOrganism(newGuid, trainingRoomSettings, Generation, ConnectionGenes.Select(gene => gene.Clone(newGuid)).ToList());
            //TODO: When inputs can mutate they should also be cloned
        }

        protected override void LoadNodes(ConnectionGene connectionGene)
        {
            ((EvaluatableConnectionGene) connectionGene).BuildStructure(this);
        }

        public override bool Equals(object obj, bool once = true)
        {
            if (!(obj is EvaluatableOrganism organism))
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
    }
}