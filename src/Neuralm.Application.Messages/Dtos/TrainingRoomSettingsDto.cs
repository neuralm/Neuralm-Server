using System;
using Neuralm.Domain.Entities.NEAT;

namespace Neuralm.Application.Messages.Dtos
{
    /// <summary>
    /// Represents the data transfer object for the <see cref="TrainingRoomSettings"/> class.
    /// </summary>
    public class TrainingRoomSettingsDto
    {
        /// <inheritdoc cref="TrainingRoomSettings.Id"/>
        public Guid Id { get; set; }

        /// <inheritdoc cref="TrainingRoomSettings.BrainCount"/>
        public uint BrainCount { get; set; }

        /// <inheritdoc cref="TrainingRoomSettings.InputCount"/>
        public uint InputCount { get; set; }

        /// <inheritdoc cref="TrainingRoomSettings.OutputCount"/>
        public uint OutputCount { get; set; }

        /// <inheritdoc cref="TrainingRoomSettings.SpeciesExcessGeneWeight"/>
        public double SpeciesExcessGeneWeight { get; set; }

        /// <inheritdoc cref="TrainingRoomSettings.SpeciesDisjointGeneWeight"/>
        public double SpeciesDisjointGeneWeight { get; set; }

        /// <inheritdoc cref="TrainingRoomSettings.SpeciesAverageWeightDiffWeight"/>
        public double SpeciesAverageWeightDiffWeight { get; set; }

        /// <inheritdoc cref="TrainingRoomSettings.Threshold"/>
        public double Threshold { get; set; }

        /// <inheritdoc cref="TrainingRoomSettings.AddConnectionChance"/>
        public double AddConnectionChance { get; set; }

        /// <inheritdoc cref="TrainingRoomSettings.AddNodeChance"/>
        public double AddNodeChance { get; set; }

        /// <inheritdoc cref="TrainingRoomSettings.CrossOverChance"/>
        public double CrossOverChance { get; set; }

        /// <inheritdoc cref="TrainingRoomSettings.InterSpeciesChance"/>
        public double InterSpeciesChance { get; set; }

        /// <inheritdoc cref="TrainingRoomSettings.MutationChance"/>
        public double MutationChance { get; set; }

        /// <inheritdoc cref="TrainingRoomSettings.MutateWeightChance"/>
        public double MutateWeightChance { get; set; }

        /// <inheritdoc cref="TrainingRoomSettings.WeightReassignChance"/>
        public double WeightReassignChance { get; set; }

        /// <inheritdoc cref="TrainingRoomSettings.TopAmountToSurvive"/>
        public double TopAmountToSurvive { get; set; }

        /// <inheritdoc cref="TrainingRoomSettings.EnableConnectionChance"/>
        public double EnableConnectionChance { get; set; }

        /// <inheritdoc cref="TrainingRoomSettings.Seed"/>
        public int Seed { get; set; }
    }
}
