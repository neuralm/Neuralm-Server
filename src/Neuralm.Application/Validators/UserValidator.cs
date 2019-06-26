using Neuralm.Application.Exceptions;
using Neuralm.Application.Interfaces;
using Neuralm.Domain.Entities;

namespace Neuralm.Application.Validators
{
    public class UserValidator : IEntityValidator<User>
    {
        public bool Validate(User entity)
        {
            if (entity == null)
                throw new EntityValidationException("User is null");
            if (string.IsNullOrWhiteSpace(entity.Username))
                throw new EntityValidationException("Username IsNullOrWhiteSpace.");
            if (!entity.TimestampCreated.Equals(default))
                throw new EntityValidationException("TimestampCreated is set, only the database is allowed to set Date related properties.");
            return true;
        }
    }
}
