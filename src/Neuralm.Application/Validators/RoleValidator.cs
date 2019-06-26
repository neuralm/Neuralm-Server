using Neuralm.Application.Exceptions;
using Neuralm.Application.Interfaces;
using Neuralm.Domain.Entities.Authentication;

namespace Neuralm.Application.Validators
{
    public class RoleValidator : IEntityValidator<Role>
    {
        public bool Validate(Role entity)
        {
            if (entity == null)
                throw new EntityValidationException("Role is null");
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
