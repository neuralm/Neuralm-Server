namespace Neuralm.Domain.Entities.NEAT
{
    public class TrainingRoomSettings
    {
        public int BrainCount { get; }
        public int InputCount { get; }
        public int OutputCount { get; }
        public double C1 { get; }
        public double C2 { get; }
        public double C3 { get; }
        public double Threshold { get; }
        public double AddConnectionChance { get; }
        public double AddNodeChance { get; }
        public double CrossOverChance { get; }
        public double InterSpeciesChance { get; }
        public double MutationChance { get; }
        public double MutateWeightChance { get; }
        public double WeightReassignChance { get; }
        public double TopAmountToSurvive { get; }
        public double EnableConnectionChance { get; }

        public TrainingRoomSettings(
            int brainCount, int inputCount, int outputCount,
            double c1, double c2, double c3,
            double threshold, double addConnectionChance, double addNodeChance,
            double crossOverChance, double interSpeciesChance, double mutationChance,
            double mutateWeightChance, double weightReassignChance, double topAmountToSurvive, double enableConnectionChance)
        {
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
        }
    }
}
