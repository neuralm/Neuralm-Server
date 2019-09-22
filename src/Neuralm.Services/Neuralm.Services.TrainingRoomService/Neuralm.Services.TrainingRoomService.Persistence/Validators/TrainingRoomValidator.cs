using Neuralm.Services.Common.Persistence;
using Neuralm.Services.Common.Persistence.Exceptions;
using Neuralm.Services.TrainingRoomService.Domain;
using System;

namespace Neuralm.Services.TrainingRoomService.Persistence.Validators
{
    /// <summary>
    /// Represents the <see cref="TrainingRoomValidator"/> class.
    /// </summary>
    public sealed class TrainingRoomValidator : IEntityValidator<TrainingRoom>
    {
        /// <inheritdoc cref="IEntityValidator{T}.Validate(T)"/>
        public bool Validate(TrainingRoom entity)
        {
            if (entity.Id.Equals(Guid.Empty))
                throw new EntityValidationException("The Id cannot be an empty guid.");
            if (entity.AuthorizedTrainers == null)
                throw new EntityValidationException("AuthorizedTrainers cannot be null.");
            if (string.IsNullOrWhiteSpace(entity.Name))
                throw new EntityValidationException("Name cannot be null or empty.");
            if (entity.Owner == null)
                throw new EntityValidationException("Owner cannot be null.");
            if (entity.TrainingRoomSettings == null)
                throw new EntityValidationException("TrainingSettings cannot be null.");
            if (entity.TrainingSessions == null)
                throw new EntityValidationException("TrainingSessions cannot be null.");
            return true;
        }
    }
}
