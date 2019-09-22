using Microsoft.EntityFrameworkCore;
using Neuralm.Services.Common.Persistence.EFCore.Infrastructure;
using Neuralm.Services.UserService.Persistence.Contexts;

namespace Neuralm.Services.UserService.Persistence.Infrastructure
{
    /// <summary>
    /// Represents the <see cref="UserDatabaseFactory"/> class.
    /// </summary>
    public class UserDatabaseFactory : DatabaseFactory<UserDbContext>
    {
        /// <summary>
        /// Creates a new instance of the <see cref="UserDbContext"/> class.
        /// </summary>
        /// <param name="dbContextOptions">The options.</param>
        /// <returns>The user database context.</returns>
        protected override UserDbContext CreateNewInstance(DbContextOptions<UserDbContext> dbContextOptions)
        {
            return new UserDbContext(dbContextOptions);
        }
    }
}
