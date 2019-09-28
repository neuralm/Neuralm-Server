using Microsoft.EntityFrameworkCore;
using Neuralm.Services.Common.Persistence.EFCore.Extensions;

namespace Neuralm.Services.RegistryService.Persistence.Contexts
{
    /// <summary>
    /// Represents the <see cref="ServiceDbContext"/> class.
    /// </summary>
    public class ServiceDbContext : DbContext
    {
        /// <summary>
        /// Initializes an instance of the <see cref="ServiceDbContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public ServiceDbContext(DbContextOptions<ServiceDbContext> options) : base(options)
        {

        }

        /// <summary>
        /// Applies all entity configurations.
        /// </summary>
        /// <param name="modelBuilder">the model builder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyAllConfigurations<ServiceDbContext>();
            base.OnModelCreating(modelBuilder);
        }
    }
}
