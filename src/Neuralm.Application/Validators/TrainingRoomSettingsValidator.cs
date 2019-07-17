using System;
using Neuralm.Application.Interfaces;
using Neuralm.Domain.Entities.NEAT;

namespace Neuralm.Application.Validators
{
    /// <inheritdoc cref="IEntityValidator{T}"/>
    public sealed class TrainingRoomSettingsValidator : IEntityValidator<TrainingRoomSettings>
    {
        public bool Validate(TrainingRoomSettings entity)
        {
            throw new NotImplementedException();
        }
    }
}
