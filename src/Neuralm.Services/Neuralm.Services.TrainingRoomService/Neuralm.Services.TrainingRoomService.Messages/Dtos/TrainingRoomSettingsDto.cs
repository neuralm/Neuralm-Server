using System;

namespace Neuralm.Services.TrainingRoomService.Messages.Dtos
{
    /// <summary>
    /// Represents the data transfer object for the TrainingRoomSettings class.
    /// </summary>
    public class TrainingRoomSettingsDto
    {
        /// <summary>
        /// Gets and sets the id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets and sets the organism count.
        /// </summary>
        public uint OrganismCount { get; set; }

        /// <summary>
        /// Gets and sets the input count.
        /// </summary>
        public uint InputCount { get; set; }

        /// <summary>
        /// Gets and sets the output count.
        /// </summary>
        public uint OutputCount { get; set; }

        /// <summary>
        /// Gets and sets the species excess gene weight.
        /// </summary>
        public double SpeciesExcessGeneWeight { get; set; }

        /// <summary>
        /// Gets and sets the species disjoint gene weight.
        /// </summary>
        public double SpeciesDisjointGeneWeight { get; set; }

        /// <summary>
        /// Gets and sets the species average weight difference weight.
        /// </summary>
        public double SpeciesAverageWeightDiffWeight { get; set; }

        /// <summary>
        /// Gets and sets the threshold.
        /// </summary>
        public double Threshold { get; set; }

        /// <summary>
        /// Gets and sets the add connection chance.
        /// </summary>
        public double AddConnectionChance { get; set; }

        /// <summary>
        /// Gets and sets the add node chance.
        /// </summary>
        public double AddNodeChance { get; set; }

        /// <summary>
        /// Gets and sets the cross over chance.
        /// </summary>
        public double CrossOverChance { get; set; }

        /// <summary>
        /// Gets and sets the inter species chance.
        /// </summary>
        public double InterSpeciesChance { get; set; }

        /// <summary>
        /// Gets and sets the mutation chance.
        /// </summary>
        public double MutationChance { get; set; }

        /// <summary>
        /// Gets and sets the mutate weight chance.
        /// </summary>
        public double MutateWeightChance { get; set; }

        /// <summary>
        /// Gets and sets the weight reassign chance.
        /// </summary>
        public double WeightReassignChance { get; set; }

        /// <summary>
        /// Gets and sets the top amount to survive.
        /// </summary>
        public double TopAmountToSurvive { get; set; }

        /// <summary>
        /// Gets and sets the enable connection chance.
        /// </summary>
        public double EnableConnectionChance { get; set; }

        /// <summary>
        /// Gets and sets the seed.
        /// </summary>
        public int Seed { get; set; }
        
        /// <summary>
        /// How long a species can be stagnant before being killed.
        /// </summary>
        public uint MaxStagnantTime { get; set; }
    }
}
