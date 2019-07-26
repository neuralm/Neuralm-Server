using System;

namespace Neuralm.Domain.Entities.NEAT
{
    /// <summary>
    /// Represents the <see cref="TrainingRoomSettings"/> class that holds settings for a <see cref="TrainingRoom"/>.
    /// </summary>
    public class TrainingRoomSettings
    {
        /// <summary>
        /// Gets and sets the id.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets and sets the brain count.
        /// </summary>
        public uint OrganismCount { get; private set; }

        /// <summary>
        /// Gets and sets the input count.
        /// </summary>
        public uint InputCount { get; private set; }

        /// <summary>
        /// Gets and sets the output count.
        /// </summary>
        public uint OutputCount { get; private set; }

        /// <summary>
        /// Gets and sets SpeciesExcessGeneWeight.
        /// The higher this value the more important the amount of excess genes is, and thus the faster that causes new species to be created
        /// </summary>
        public double SpeciesExcessGeneWeight { get; private set; }

        /// <summary>
        /// Gets and sets SpeciesDisjointGeneWeight.
        /// The higher this value the more important the amount of disjoint genes is, and thus the faster that causes new species to be created
        /// </summary>
        public double SpeciesDisjointGeneWeight { get; private set; }

        /// <summary>
        /// Gets and sets SpeciesAverageWeightDiffWeight.
        /// The higher this value the more important the difference between weights is, and thus the faster that causes new species to be created
        /// </summary>
        public double SpeciesAverageWeightDiffWeight { get; private set; }

        /// <summary>
        /// Gets and sets the threshold.
        /// </summary>
        public double Threshold { get; private set; }

        /// <summary>
        /// Gets and sets the add connection chance.
        /// </summary>
        public double AddConnectionChance { get; private set; }

        /// <summary>
        /// Gets and sets the add node chance.
        /// </summary>
        public double AddNodeChance { get; private set; }

        /// <summary>
        /// Gets and sets the cross over chance.
        /// </summary>
        public double CrossOverChance { get; private set; }

        /// <summary>
        /// Gets and sets the species chance.
        /// </summary>
        public double InterSpeciesChance { get; private set; }

        /// <summary>
        /// Gets and sets the mutation chance.
        /// </summary>
        public double MutationChance { get; private set; }

        /// <summary>
        /// Gets and sets the mutate weight chance.
        /// </summary>
        public double MutateWeightChance { get; private set; }

        /// <summary>
        /// Gets and sets the weight reassign chance.
        /// </summary>
        public double WeightReassignChance { get; private set; }

        /// <summary>
        /// Gets and sets the top amount to survive.
        /// </summary>
        public double TopAmountToSurvive { get; private set; }

        /// <summary>
        /// Gets and sets enable connection chance.
        /// </summary>
        public double EnableConnectionChance { get; private set; }

        /// <summary>
        /// Gets and sets the seed.
        /// </summary>
        public int Seed { get; private set; }

        /// <summary>
        /// EFCore entity constructor IGNORE!
        /// </summary>
        protected TrainingRoomSettings()
        {
            
        }

        /// <summary>
        /// Initializes an instance of the <see cref="TrainingRoomSettings"/> class
        /// </summary>
        /// <param name="organismCount">How many organisms each generation has.</param>
        /// <param name="inputCount">How many inputs each brain has.</param>
        /// <param name="outputCount">How many outputs each brain has.</param>
        /// <param name="c1">The importance of excess genes when checking the species. The higher this number the faster they will become a different species.</param>
        /// <param name="c2">The importance of disjoint genes when checking the species. The higher this number the faster they will become a different species.</param>
        /// <param name="c3">The average weight difference when checking the species. The higher this number the faster they will become a different species.</param>
        /// <param name="threshold">The maximum difference between 2 brains for them to count as the same species.</param>
        /// <param name="addConnectionChance">The mutation chance to add a new connection [0,1].</param>
        /// <param name="addNodeChance">The mutation chance to add a new node [0,1].</param>
        /// <param name="crossOverChance">The chance to have 2 brains crossover when generating a new brain [0,1].</param>
        /// <param name="interSpeciesChance">The chance that a crossover will be between different species [0,1].</param>
        /// <param name="mutationChance">The chance the brain will mutate at all [0,1].</param>
        /// <param name="mutateWeightChance">The chance the weight will mutate [0,1].</param>
        /// <param name="weightReassignChance">The chance the weight will randomly be assigned instead of slightly changed [0,1].</param>
        /// <param name="topAmountToSurvive">How many % of the brains in each species survive [0,1].</param>
        /// <param name="enableConnectionChance">The chance a disabled connection gets enabled when crossover happens [0,1].</param>
        /// <param name="seed">The seed for the pseudo-random generator.</param>
        public TrainingRoomSettings(
            uint organismCount, uint inputCount, uint outputCount,
            double c1, double c2, double c3,
            double threshold, double addConnectionChance, double addNodeChance,
            double crossOverChance, double interSpeciesChance, double mutationChance,
            double mutateWeightChance, double weightReassignChance, double topAmountToSurvive, double enableConnectionChance, int seed)
        {
            Id = Guid.NewGuid();
            OrganismCount = organismCount;
            InputCount = inputCount;
            OutputCount = outputCount;
            SpeciesExcessGeneWeight = c1;
            SpeciesDisjointGeneWeight = c2;
            SpeciesAverageWeightDiffWeight = c3;
            Threshold = threshold;
            AddConnectionChance = addConnectionChance;
            AddNodeChance = addNodeChance;
            CrossOverChance = crossOverChance;
            InterSpeciesChance = interSpeciesChance;
            MutationChance = mutationChance;
            MutateWeightChance = mutateWeightChance;
            WeightReassignChance = weightReassignChance;
            TopAmountToSurvive = topAmountToSurvive;
            EnableConnectionChance = enableConnectionChance;
            Seed = seed;
        }
    }
}
