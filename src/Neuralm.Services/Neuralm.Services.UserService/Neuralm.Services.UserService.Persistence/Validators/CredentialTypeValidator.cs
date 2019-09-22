using Neuralm.Services.Common.Persistence;
using Neuralm.Services.Common.Persistence.Exceptions;
using Neuralm.Services.UserService.Domain.Authentication;

namespace Neuralm.Services.UserService.Persistence.Validators
{
    /// <summary>
    /// Represents the <see cref="CredentialTypeValidator"/> class.
    /// </summary>
    public class CredentialTypeValidator : IEntityValidator<CredentialType>
    {
        /// <inheritdoc cref="IEntityValidator{T}.Validate(T)"/>
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
