using System;
using Neuralm.Application.Interfaces;
using Neuralm.Domain.Entities.NEAT;

namespace Neuralm.Application.Validators
{
    /// <summary>
    /// Represents the <see cref="TrainingRoomSettingsValidator"/> class.
    /// </summary>
    public sealed class TrainingRoomSettingsValidator : IEntityValidator<TrainingRoomSettings>
    {
        /// <inheritdoc cref="IEntityValidator{T}.Validate(T)"/>
        public bool Validate(TrainingRoomSettings entity)
        {
            throw new NotImplementedException();
        }
    }
}
