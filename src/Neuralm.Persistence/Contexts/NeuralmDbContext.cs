using Microsoft.EntityFrameworkCore;
using Neuralm.Domain.Entities;
using Neuralm.Domain.Entities.Authentication;
using Neuralm.Domain.Entities.NEAT;
using Neuralm.Persistence.Extensions;

namespace Neuralm.Persistence.Contexts
{
    /// <summary>
    /// Represents the <see cref="NeuralmDbContext"/> class; i.e. the database for Neuralm.
    /// </summary>
    public class NeuralmDbContext : DbContext
    {
        /// <summary>
        /// Gets and sets the users.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Gets and sets the credential types.
        /// </summary>
        public DbSet<CredentialType> CredentialTypes { get; set; }

        /// <summary>
        /// Gets and sets the credentials.
        /// </summary>
        public DbSet<Credential> Credentials { get; set; }

        /// <summary>
        /// Gets and sets the roles.
        /// </summary>
        public DbSet<Role> Roles { get; set; }

        /// <summary>
        /// Gets and sets the user roles.
        /// </summary>
        public DbSet<UserRole> UserRoles { get; set; }

        /// <summary>
        /// Gets and sets permissions.
        /// </summary>
        public DbSet<Permission> Permissions { get; set; }

        /// <summary>
        /// Gets and sets role permissions.
        /// </summary>
        public DbSet<RolePermission> RolePermissions { get; set; }

        /// <summary>
        /// Gets and sets training rooms.
        /// </summary>
        public DbSet<TrainingRoom> TrainingRooms { get; set; }

        /// <summary>
        /// Gets and sets training room settings.
        /// </summary>
        public DbSet<TrainingRoomSettings> TrainingRoomSettings { get; set; }

        /// <summary>
        /// Gets and sets connection genes.
        /// </summary>
        public DbSet<ConnectionGene> ConnectionGenes { get; set; }

        /// <summary>
        /// Gets and sets brains.
        /// </summary>
        public DbSet<Brain> Brains { get; set; }

        /// <summary>
        /// Gets and sets training sessions.
        /// </summary>
        public DbSet<TrainingSession> TrainingSessions { get; set; }

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
