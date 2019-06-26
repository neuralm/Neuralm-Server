using Neuralm.Application.Interfaces;
using Neuralm.Domain.Entities;
using Neuralm.Persistence.Abstractions;
using Neuralm.Persistence.Contexts;

namespace Neuralm.Persistence.Repositories
{
    public class UserRepository : RepositoryBase<User, NeuralmDbContext>
    {
        public UserRepository(NeuralmDbContext dbContext, IEntityValidator<User> entityValidator) : base(dbContext, entityValidator)
        {
        }
    }
}
