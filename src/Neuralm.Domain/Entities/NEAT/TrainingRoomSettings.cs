using System;

namespace Neuralm.Domain.Entities.NEAT
{
    public class TrainingRoomSettings
    {
        public Guid Id { get; private set; }
        public uint BrainCount { get; private set; }
        public uint InputCount { get; private set; }
        public uint OutputCount { get; private set; }
        public double C1 { get; private set; }
        public double C2 { get; private set; }
        public double C3 { get; private set; }
        public double Threshold { get; private set; }
        public double AddConnectionChance { get; private set; }
        public double AddNodeChance { get; private set; }
        public double CrossOverChance { get; private set; }
        public double InterSpeciesChance { get; private set; }
        public double MutationChance { get; private set; }
        public double MutateWeightChance { get; private set; }
        public double WeightReassignChance { get; private set; }
        public double TopAmountToSurvive { get; private set; }
        public double EnableConnectionChance { get; private set; }
        public int Seed { get; private set; }
        
        /// <summary>
        /// EFCore entity constructor IGNORE!
        /// </summary>
        protected TrainingRoomSettings()
        {
            
        }

        /// <summary>
        /// The settings for the training room.
        /// </summary>
        /// <param name="brainCount">How many brains each generation has</param>
        /// <param name="inputCount">How many inputs each brain has</param>
        /// <param name="outputCount">How many outputs each brain has</param>
        /// <param name="c1">The importance of excess genes when checking the species. The higher this number the faster they will become a different species</param>
        /// <param name="c2">The importance of disjoint genes when checking the species. The higher this number the faster they will become a different species</param>
        /// <param name="c3">The average weight difference when checking the species. The higher this number the faster they will become a different species</param>
        /// <param name="threshold">The maximum difference between 2 brains for them to count as the same species.</param>
        /// <param name="addConnectionChance">The mutation chance to add a new connection [0,1]</param>
        /// <param name="addNodeChance">The mutation chance to add a new node [0,1]</param>
        /// <param name="crossOverChance">The chance to have 2 brains crossover when generating a new brain [0,1]</param>
        /// <param name="interSpeciesChance">The chance that a crossover will be between different species [0,1]</param>
        /// <param name="mutationChance">The chance the brain will mutate at all [0,1]</param>
        /// <param name="mutateWeightChance">The chance the weight will mutate [0,1]</param>
        /// <param name="weightReassignChance">The chance the weight will randomly be assigned instead of slightly changed [0,1]</param>
        /// <param name="topAmountToSurvive">How many % of the brains in each species survive [0,1]</param>
        /// <param name="enableConnectionChance">The chance a disabled connection gets enabled when crossover happens [0,1]</param>
        /// <param name="seed">The seed for the pseudo-random generator</param>
        public TrainingRoomSettings(
            uint brainCount, uint inputCount, uint outputCount,
            double c1, double c2, double c3,
            double threshold, double addConnectionChance, double addNodeChance,
            double crossOverChance, double interSpeciesChance, double mutationChance,
            double mutateWeightChance, double weightReassignChance, double topAmountToSurvive, double enableConnectionChance, int seed)
        {
            Id = Guid.NewGuid();
            BrainCount = brainCount;
            InputCount = inputCount;
            OutputCount = outputCount;
            C1 = c1;
            C2 = c2;
            C3 = c3;
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
