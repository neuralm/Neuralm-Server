using Microsoft.EntityFrameworkCore;
using Neuralm.Domain.Entities;
using Neuralm.Domain.Entities.Authentication;
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

        public NeuralmDbContext(DbContextOptions<NeuralmDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyAllConfigurations();
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder builder)
        //{
        //    builder.UseSqlServer("Server=(LocalDb)\\MSSQLLocalDB;Database=NeuralmDbContext;Trusted_Connection=True;MultipleActiveResultSets=true");
        //    base.OnConfiguring(builder);
        //}
    }
}
