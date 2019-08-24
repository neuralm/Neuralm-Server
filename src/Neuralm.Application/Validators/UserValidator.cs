using Neuralm.Application.Exceptions;
using Neuralm.Application.Interfaces;
using Neuralm.Domain.Entities;

namespace Neuralm.Application.Validators
{
    /// <summary>
    /// Represents the <see cref="UserValidator"/> class.
    /// </summary>
    public class UserValidator : IEntityValidator<User>
    {
        /// <inheritdoc cref="IEntityValidator{T}.Validate(T)"/>
        public bool Validate(User entity)
        {
            if (entity == null)
                throw new EntityValidationException("User is null");
            if (string.IsNullOrWhiteSpace(entity.Username))
                throw new EntityValidationException("Username IsNullOrWhiteSpace.");
            if (entity.TimestampCreated.Equals(default))
                throw new EntityValidationException("TimestampCreated is not set.");
            return true;
        }
    }
}
