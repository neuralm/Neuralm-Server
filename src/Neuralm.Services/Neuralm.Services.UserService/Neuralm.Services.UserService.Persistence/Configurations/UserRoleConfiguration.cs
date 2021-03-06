﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neuralm.Services.UserService.Domain.Authentication;

namespace Neuralm.Services.UserService.Persistence.Configurations
{
    /// <summary>
    /// Represents the <see cref="UserRoleConfiguration"/> class used to configure the relations and columns in the <see cref="DbSet{TEntity}"/> for <see cref="UserRole"/> in the DbContext.
    /// </summary>
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        /// <inheritdoc cref="IEntityTypeConfiguration{TEntity}.Configure"/>
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.Ignore(e => e.Id);
            builder.HasKey(e => new { e.UserId, e.RoleId });
        }
    }
}
