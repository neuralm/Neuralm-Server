using System;

namespace Neuralm.Application.Messages.Dtos
{
    public class TrainingRoomSettingsDto
    {
        public Guid Id { get; set; }
        public uint BrainCount { get; set; }
        public uint InputCount { get; set; }
        public uint OutputCount { get; set; }
        public double C1 { get; set; }
        public double C2 { get; set; }
        public double C3 { get; set; }
        public double Threshold { get; set; }
        public double AddConnectionChance { get; set; }
        public double AddNodeChance { get; set; }
        public double CrossOverChance { get; set; }
        public double InterSpeciesChance { get; set; }
        public double MutationChance { get; set; }
        public double MutateWeightChance { get; set; }
        public double WeightReassignChance { get; set; }
        public double TopAmountToSurvive { get; set; }
        public double EnableConnectionChance { get; set; }
        public int Seed { get; set; }
    }
}
