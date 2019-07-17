﻿using System;
using Neuralm.Application.Exceptions;
using Neuralm.Application.Interfaces;
using Neuralm.Domain.Entities.NEAT;

namespace Neuralm.Application.Validators
{
    /// <summary>
    /// Represents the <see cref="TrainingSessionValidator"/> class.
    /// </summary>
    public sealed class TrainingSessionValidator : IEntityValidator<TrainingSession>
    {
        /// <inheritdoc cref="IEntityValidator{T}.Validate(T)"/>
        public bool Validate(TrainingSession entity)
        {
            if (entity.Id.Equals(Guid.Empty))
                throw new EntityValidationException("Id cannot be empty guid.");
            if (entity.StartedTimestamp == default)
                throw new EntityValidationException("StartedTimestamp cannot be default.");
            if (entity.TrainingRoom == null)
                throw new EntityValidationException("TrainingRoom cannot be null.");
            if (entity.UserId.Equals(Guid.Empty))
                throw new EntityValidationException("UserId cannot be an empty guid.");
            return true;
        }
    }
}
