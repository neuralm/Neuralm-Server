using Neuralm.Services.Common.Persistence;
using Neuralm.Services.Common.Persistence.Exceptions;
using Neuralm.Services.UserService.Domain.Authentication;

namespace Neuralm.Services.UserService.Persistence.Validators
{
    /// <summary>
    /// Represents the <see cref="CredentialValidator"/> class.
    /// </summary>
    public class CredentialValidator : IEntityValidator<Credential>
    {
        /// <inheritdoc cref="IEntityValidator{T}.Validate(T)"/>
        public bool Validate(Credential entity)
        {
            if (entity == null)
                throw new EntityValidationException("Credential is null");
            if (!entity.Id.Equals(default))
                throw new EntityValidationException("Id cannot be set.");
            if (entity.UserId.Equals(default))
                throw new EntityValidationException("UserId is unset.");
            if (entity.CredentialTypeId.Equals(default))
                throw new EntityValidationException("CredentialTypeId is unset.");
            if (string.IsNullOrWhiteSpace(entity.Identifier))
                throw new EntityValidationException("Identifier IsNullOrWhiteSpace.");
            if (string.IsNullOrWhiteSpace(entity.Secret))
                throw new EntityValidationException("Secret IsNullOrWhiteSpace.");
            if (string.IsNullOrWhiteSpace(entity.Extra))
                throw new EntityValidationException("Extra IsNullOrWhiteSpace.");
            return true;
        }
    }
}
