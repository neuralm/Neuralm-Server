using System;
using Neuralm.Services.Common.Patterns;
using Neuralm.Services.TrainingRoomService.Domain.FactoryArguments;

namespace Neuralm.Services.TrainingRoomService.Domain
{
    /// <summary>
    /// Represents the <see cref="OrganismFactory"/> class.
    /// Used for constructing organisms.
    /// </summary>
    public class OrganismFactory : IFactory<Organism, OrganismFactoryArgument>
    {
        /// <inheritdoc cref="IFactory{Organism, OrganismFactoryArgument}.Create(OrganismFactoryArgument)"/>
        public Organism Create(OrganismFactoryArgument argument)
        {
            return argument.CreationType switch
                {
                OrganismCreationType.NEW => new Organism(argument.Generation, argument.TrainingRoomSettings),
                OrganismCreationType.NEW_WITH_GENES => new Organism(argument.Id, argument.TrainingRoomSettings, argument.Generation, argument.ConnectionGenes),
                _ => throw new ArgumentOutOfRangeException()
                };
        }
    }
}
