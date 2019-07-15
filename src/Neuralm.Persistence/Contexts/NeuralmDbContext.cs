using Microsoft.EntityFrameworkCore;
using Neuralm.Domain.Entities;
using Neuralm.Domain.Entities.Authentication;
using Neuralm.Domain.Entities.NEAT;
using Neuralm.Persistence.Extensions;

namespace Neuralm.Persistence.Contexts
{
    public class NeuralmDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<CredentialType> CredentialTypes { get; set; }
        public DbSet<Credential> Credentials { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<TrainingRoom> TrainingRooms { get; set; }
        public DbSet<TrainingRoomSettings> TrainingRoomSettings { get; set; }

        public NeuralmDbContext(DbContextOptions<NeuralmDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyAllConfigurations();
            modelBuilder.Entity<CredentialType>().HasData(new CredentialType { Name = "Name", Code = "Name", Position = 1, Id = 1 });
            base.OnModelCreating(modelBuilder);
        }
    }
}
