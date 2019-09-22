using Microsoft.EntityFrameworkCore;
using Neuralm.Domain.Entities.Authentication;
using Neuralm.Persistence.Extensions;

namespace Neuralm.Persistence.Contexts
{
    /// <summary>
    /// Represents the <see cref="NeuralmDbContext"/> class; i.e. the database for Neuralm.
    /// </summary>
    public class NeuralmDbContext : DbContext
    {
        /// <summary>
        /// Initializes an instance of the <see cref="NeuralmDbContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public NeuralmDbContext(DbContextOptions<NeuralmDbContext> options) : base(options)
        {
            
        }

        /// <summary>
        /// Applies all entity configurations and seeds a default <see cref="CredentialType"/> Name.
        /// </summary>
        /// <param name="modelBuilder">the model builder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyAllConfigurations();
            modelBuilder.Entity<CredentialType>().HasData(new CredentialType { Name = "Name", Code = "Name", Position = 1, Id = 1 });
            base.OnModelCreating(modelBuilder);
        }
    }
}
