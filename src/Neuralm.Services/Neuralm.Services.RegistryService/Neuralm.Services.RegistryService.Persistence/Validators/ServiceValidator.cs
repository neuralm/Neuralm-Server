using Neuralm.Services.Common.Persistence;
using Neuralm.Services.Common.Persistence.Exceptions;
using Neuralm.Services.Common.Persistence.Extensions;
using Neuralm.Services.RegistryService.Domain;
using System;

namespace Neuralm.Services.RegistryService.Persistence.Validators
{
    /// <summary>
    /// Represents the <see cref="ServiceValidator"/> class.
    /// </summary>
    public sealed class ServiceValidator : IEntityValidator<Service>
    {
        /// <inheritdoc cref="IEntityValidator{T}.Validate(T)"/>
        public bool Validate(Service entity)
        {
            if (entity.Id.Equals(Guid.Empty))
                throw new EntityValidationException("Id cannot be empty guid.");
            if (entity.Start == default)
                throw new EntityValidationException("Start cannot be default.");
            if (entity.End != default)
                throw new EntityValidationException("End must be null upon creation.");
            if (string.IsNullOrWhiteSpace(entity.Name))
                throw new EntityValidationException("Name cannot be null or empty.");
            if (ValidationUtilities.IsUrlValid(entity.Host))
                throw new EntityValidationException("Host is invalid.");
            return true;
        }
    }
}
