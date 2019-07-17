using Neuralm.Application.Exceptions;
using Neuralm.Application.Interfaces;
using Neuralm.Domain.Entities.Authentication;

namespace Neuralm.Application.Validators
{
    /// <inheritdoc cref="IEntityValidator{T}"/>
    public class CredentialTypeValidator : IEntityValidator<CredentialType>
    {
        public bool Validate(CredentialType entity)
        {
            if (entity == null)
                throw new EntityValidationException("CredentialType is null");
            if (!entity.Id.Equals(default))
                throw new EntityValidationException("Id cannot be set.");
            if (string.IsNullOrWhiteSpace(entity.Code))
                throw new EntityValidationException("Code IsNullOrWhiteSpace.");
            if (string.IsNullOrWhiteSpace(entity.Name))
                throw new EntityValidationException("Name IsNullOrWhiteSpace.");
            return true;
        }
    }
}
