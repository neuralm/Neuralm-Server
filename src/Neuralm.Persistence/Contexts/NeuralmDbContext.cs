using Microsoft.EntityFrameworkCore;
using Neuralm.Domain.Entities;
using Neuralm.Domain.Entities.Authentication;

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

        public NeuralmDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
