using Neuralm.Application.Exceptions;
using Neuralm.Application.Interfaces;
using Neuralm.Domain.Entities.Authentication;

namespace Neuralm.Application.Validators
{
    public class RolePermissionValidator : IEntityValidator<RolePermission>
    {
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
