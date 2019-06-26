using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Neuralm.Application.Configurations;
using Neuralm.Application.Cryptography;
using Neuralm.Application.Interfaces;
using Neuralm.Application.Services;
using Neuralm.Application.Validators;
using Neuralm.Domain.Entities;
using Neuralm.Domain.Entities.Authentication;
using Neuralm.Persistence.Contexts;
using Neuralm.Persistence.Infrastructure;
using Neuralm.Persistence.Repositories;

namespace Neuralm.Mapping
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IFactory<NeuralmDbContext>, NeuralmDbFactory>();

            #region Cryptography
            serviceCollection.AddSingleton<IHasher, Pbkdf2Hasher>();
            serviceCollection.AddSingleton<ISaltGenerator, RandomSaltGenerator>();
            #endregion Cryptography

            #region Validators
            serviceCollection.AddTransient<IEntityValidator<User>, UserValidator>();
            serviceCollection.AddTransient<IEntityValidator<Credential>, CredentialValidator>();
            serviceCollection.AddTransient<IEntityValidator<CredentialType>, CredentialTypeValidator>();
            serviceCollection.AddTransient<IEntityValidator<UserRole>, UserRoleValidator>();
            serviceCollection.AddTransient<IEntityValidator<Role>, RoleValidator>();
            serviceCollection.AddTransient<IEntityValidator<RolePermission>, RolePermissionValidator>();
            serviceCollection.AddTransient<IEntityValidator<Permission>, PermissionValidator>();
            #endregion Validators

            #region Repositories
            serviceCollection.AddTransient<IRepository<User>, UserRepository>(serviceProvider =>
            {
                IFactory<NeuralmDbContext> neuralmDbFactory = serviceProvider.GetService<IFactory<NeuralmDbContext>>();
                IEntityValidator<User> entityValidator = serviceProvider.GetService<IEntityValidator<User>>();
                return new UserRepository(neuralmDbFactory.Create(), entityValidator);
            });
            serviceCollection.AddTransient<IRepository<Credential>, CredentialRepository>(serviceProvider =>
            {
                IFactory<NeuralmDbContext> neuralmDbFactory = serviceProvider.GetService<IFactory<NeuralmDbContext>>();
                IEntityValidator<Credential> entityValidator = serviceProvider.GetService<IEntityValidator<Credential>>();
                return new CredentialRepository(neuralmDbFactory.Create(), entityValidator);
            });
            serviceCollection.AddTransient<IRepository<CredentialType>, CredentialTypeRepository>(serviceProvider =>
            {
                IFactory<NeuralmDbContext> neuralmDbFactory = serviceProvider.GetService<IFactory<NeuralmDbContext>>();
                IEntityValidator<CredentialType> entityValidator = serviceProvider.GetService<IEntityValidator<CredentialType>>();
                return new CredentialTypeRepository(neuralmDbFactory.Create(), entityValidator);
            });
            serviceCollection.AddTransient<IRepository<UserRole>, UserRoleRepository>(serviceProvider =>
            {
                IFactory<NeuralmDbContext> neuralmDbFactory = serviceProvider.GetService<IFactory<NeuralmDbContext>>();
                IEntityValidator<UserRole> entityValidator = serviceProvider.GetService<IEntityValidator<UserRole>>();
                return new UserRoleRepository(neuralmDbFactory.Create(), entityValidator);
            });
            serviceCollection.AddTransient<IRepository<Role>, RoleRepository>(serviceProvider =>
            {
                IFactory<NeuralmDbContext> neuralmDbFactory = serviceProvider.GetService<IFactory<NeuralmDbContext>>();
                IEntityValidator<Role> entityValidator = serviceProvider.GetService<IEntityValidator<Role>>();
                return new RoleRepository(neuralmDbFactory.Create(), entityValidator);
            });
            serviceCollection.AddTransient<IRepository<RolePermission>, RolePermissionRepository>(serviceProvider =>
            {
                IFactory<NeuralmDbContext> neuralmDbFactory = serviceProvider.GetService<IFactory<NeuralmDbContext>>();
                IEntityValidator<RolePermission> entityValidator = serviceProvider.GetService<IEntityValidator<RolePermission>>();
                return new RolePermissionRepository(neuralmDbFactory.Create(), entityValidator);
            });
            serviceCollection.AddTransient<IRepository<Permission>, PermissionRepository>(serviceProvider =>
            {
                IFactory<NeuralmDbContext> neuralmDbFactory = serviceProvider.GetService<IFactory<NeuralmDbContext>>();
                IEntityValidator<Permission> entityValidator = serviceProvider.GetService<IEntityValidator<Permission>>();
                return new PermissionRepository(neuralmDbFactory.Create(), entityValidator);
            });
            #endregion Repositories

            #region Services
            serviceCollection.AddSingleton<IAccessTokenService, JwtAccessTokenService>();
            serviceCollection.AddTransient<IUserService, UserService>();
            #endregion Services
            
            return serviceCollection;
        }

        public static IServiceCollection AddConfigurations(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddOptions();
            serviceCollection.Configure<ServerConfiguration>(configuration.GetSection("Server").Bind);
            serviceCollection.Configure<DbConfiguration>(configuration.GetSection("NeuralmDb").Bind);
            serviceCollection.Configure<JwtConfiguration>(configuration.GetSection("Jwt").Bind);
            return serviceCollection;
        }

        public static IGenericServiceProvider ToGenericServiceProvider(this IServiceProvider serviceProvider)
        {
            return new GenericServiceProvider(serviceProvider);
        }
    }
}
