using System;
using Neuralm.Services.Common.Patterns;
using Neuralm.Services.TrainingRoomService.Domain;
using Neuralm.Services.TrainingRoomService.Domain.FactoryArguments;

namespace Neuralm.Services.TrainingRoomService.Domain.Evaluatables
{
    public class EvaluatableOrganismFactory : IFactory<Organism, OrganismFactoryArgument>
    {
        public Organism Create(OrganismFactoryArgument argument)
        {
            return argument.CreationType switch
                {
                OrganismCreationType.NEW => new EvaluatableOrganism(argument.Generation, argument.TrainingRoomSettings),
                OrganismCreationType.NEW_WITH_GENES => new EvaluatableOrganism(argument.Id, argument.TrainingRoomSettings, argument.Generation, argument.ConnectionGenes),
                _ => throw new ArgumentOutOfRangeException()
                };
        }
    }
}