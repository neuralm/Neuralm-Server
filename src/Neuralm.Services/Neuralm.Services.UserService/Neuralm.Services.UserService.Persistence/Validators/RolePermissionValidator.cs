using Neuralm.Services.Common.Persistence;
using Neuralm.Services.Common.Persistence.Exceptions;
using Neuralm.Services.UserService.Domain.Authentication;

namespace Neuralm.Services.UserService.Persistence.Validators
{
    /// <summary>
    /// Represents the <see cref="RolePermissionValidator"/> class.
    /// </summary>
    public class RolePermissionValidator : IEntityValidator<RolePermission>
    {
        /// <inheritdoc cref="IEntityValidator{T}.Validate(T)"/>
        public bool Validate(RolePermission entity)
        {
            if (entity == null)
                throw new EntityValidationException("RoleId is null");
            if (entity.RoleId.Equals(default))
                throw new EntityValidationException("RoleId is unset.");
            if (entity.PermissionId.Equals(default))
                throw new EntityValidationException("PermissionId is unset.");
            return true;
        }
    }
}
