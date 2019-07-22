using System;
using Neuralm.Application.Exceptions;
using Neuralm.Application.Interfaces;
using Neuralm.Domain.Entities.NEAT;

namespace Neuralm.Application.Validators
{
    /// <summary>
    /// Represents the <see cref="BrainValidator"/> class.
    /// </summary>
    public sealed class BrainValidator : IEntityValidator<Brain>
    {
        /// <inheritdoc cref="IEntityValidator{T}.Validate(T)"/>
        public bool Validate(Brain entity)
        {
            if (entity.Id.Equals(Guid.Empty))
                throw new EntityValidationException("Id cannot be empty guid.");
            if (entity.ConnectionGenes == null)
                throw new EntityValidationException("Genes cannot be null");
            return true;
        }
    }
}
