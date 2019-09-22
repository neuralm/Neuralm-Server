using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neuralm.Services.UserService.Domain.Authentication;

namespace Neuralm.Services.UserService.Persistence.Configurations
{
    /// <summary>
    /// Represents the <see cref="RolePermissionConfiguration"/> class used to configure the relations and columns in the <see cref="DbSet{TEntity}"/> for <see cref="RolePermission"/> in the DbContext.
    /// </summary>
    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        /// <inheritdoc cref="IEntityTypeConfiguration{TEntity}.Configure"/>
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.Ignore(e => e.Id);
            builder.HasKey(e => new { e.RoleId, e.PermissionId });
        }
    }
}
