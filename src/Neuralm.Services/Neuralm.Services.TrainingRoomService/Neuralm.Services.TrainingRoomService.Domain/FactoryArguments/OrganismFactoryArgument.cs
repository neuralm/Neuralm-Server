using System;
using System.Collections.Generic;

namespace Neuralm.Services.TrainingRoomService.Domain.FactoryArguments
{
    /// <summary>
    /// Represents the <see cref="OrganismFactoryArgument"/> struct.
    /// Used for constructing an organism with the <see cref="OrganismFactory"/> class.
    /// </summary>
    public struct OrganismFactoryArgument
    {
        /// <summary>
        /// Gets and sets the id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets and sets the training room settings.
        /// </summary>
        public TrainingRoomSettings TrainingRoomSettings { get; set; }

        /// <summary>
        /// Gets and sets the connection genes.
        /// </summary>
        public List<ConnectionGene> ConnectionGenes { get; set; }

        /// <summary>
        /// Gets and sets the generation.
        /// </summary>
        public uint Generation { get; set; }

        /// <summary>
        /// Gets and sets the organism creation type.
        /// </summary>
        public OrganismCreationType CreationType { get; set; }
    }

    /// <summary>
    /// Represents the <see cref="OrganismCreationType"/> enumeration.
    /// </summary>
    public enum OrganismCreationType
    {
        NEW,
        NEW_WITH_GENES
    }
}
