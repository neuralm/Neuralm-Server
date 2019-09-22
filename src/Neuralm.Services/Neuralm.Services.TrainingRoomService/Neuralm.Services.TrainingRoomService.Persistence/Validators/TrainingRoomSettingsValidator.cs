using Neuralm.Services.Common.Persistence;
using Neuralm.Services.TrainingRoomService.Domain;
using System;

namespace Neuralm.Services.TrainingRoomService.Persistence.Validators
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
