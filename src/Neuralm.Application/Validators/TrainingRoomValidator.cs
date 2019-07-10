﻿using System;
using Neuralm.Application.Exceptions;
using Neuralm.Application.Interfaces;
using Neuralm.Domain.Entities.NEAT;

namespace Neuralm.Application.Validators
{
    public sealed class TrainingRoomValidator : IEntityValidator<TrainingRoom>
    {
        public bool Validate(TrainingRoom entity)
        {
            if (entity.Id.Equals(Guid.Empty))
                throw new EntityValidationException("The Id cannot be an empty guid.");
            if (entity.AuthorizedUsers == null)
                throw new EntityValidationException("AuthorizedUsers cannot be null.");
            if (string.IsNullOrWhiteSpace(entity.Name))
                throw new EntityValidationException("Name cannot be null or empty.");
            if (entity.Owner == null)
                throw new EntityValidationException("Owner cannot be null.");
            if (entity.OwnerId.Equals(Guid.Empty))
                throw new EntityValidationException("The OwnerId cannot be an empty guid.");
            if (entity.TrainingRoomSettings == null)
                throw new EntityValidationException("TrainingSettings cannot be null.");
            if (entity.TrainingSessions == null)
                throw new EntityValidationException("TrainingSessions cannot be null.");
            return true;
        }
    }
}
