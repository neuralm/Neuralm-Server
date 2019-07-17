using System;
using Neuralm.Application.Exceptions;
using Neuralm.Application.Interfaces;
using Neuralm.Domain.Entities.NEAT;

namespace Neuralm.Application.Validators
{
   /// <inheritdoc cref="IEntityValidator{T}"/>
   public sealed class BrainValidator : IEntityValidator<Brain>
    {
        public bool Validate(Brain entity)
        {
            if (entity.Id.Equals(Guid.Empty))
                throw new EntityValidationException("Id cannot be empty guid.");
            if (entity.Genes == null)
                throw new EntityValidationException("Genes cannot be null");
            return true;
        }
    }
}
