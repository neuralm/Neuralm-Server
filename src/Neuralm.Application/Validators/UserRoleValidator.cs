using Neuralm.Application.Exceptions;
using Neuralm.Application.Interfaces;
using Neuralm.Domain.Entities.Authentication;

namespace Neuralm.Application.Validators
{
    /// <summary>
    /// Represents the <see cref="UserRoleValidator"/> class.
    /// </summary>
    public class UserRoleValidator : IEntityValidator<UserRole>
    {
        /// <inheritdoc cref="IEntityValidator{T}.Validate(T)"/>
        public bool Validate(UserRole entity)
        {
            if (entity == null)
                throw new EntityValidationException("UserRole is null");
            if (entity.UserId.Equals(default))
                throw new EntityValidationException("UserId cannot be 0.");
            if (entity.RoleId.Equals(default))
                throw new EntityValidationException("RoleId cannot be 0.");
            return true;
        }
    }
}
