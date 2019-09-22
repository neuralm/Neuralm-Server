using Microsoft.EntityFrameworkCore;
using Neuralm.Services.Common.Persistence.EFCore.Extensions;
using Neuralm.Services.UserService.Domain.Authentication;
using System;

namespace Neuralm.Services.UserService.Persistence.Contexts
{
    /// <summary>
    /// Represents the <see cref="UserDbContext"/> class.
    /// </summary>
    public class UserDbContext : DbContext
    {
        /// <summary>
        /// Initializes an instance of the <see cref="UserDbContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {

        }

        /// <summary>
        /// Applies all entity configurations and seeds a default <see cref="CredentialType"/> Name.
        /// </summary>
        /// <param name="modelBuilder">the model builder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyAllConfigurations<UserDbContext>();
            modelBuilder.Entity<CredentialType>().HasData(new CredentialType { Name = "Name", Code = "Name", Position = 1, Id = Guid.NewGuid() });
            base.OnModelCreating(modelBuilder);
        }
    }
}
