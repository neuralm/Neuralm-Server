using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Neuralm.Services.Common.Application.Interfaces;
using Neuralm.Services.Common.Mapping;
using Neuralm.Services.Common.Persistence;
using Neuralm.Services.Common.Persistence.EFCore.Repositories;
using Neuralm.Services.UserService.Application.Cryptography;
using Neuralm.Services.UserService.Application.Interfaces;
using Neuralm.Services.UserService.Domain;
using Neuralm.Services.UserService.Domain.Authentication;
using Neuralm.Services.UserService.Persistence.Contexts;
using Neuralm.Services.UserService.Persistence.Infrastructure;
using Neuralm.Services.UserService.Persistence.Validators;
using System.Reflection;
using Neuralm.Services.Common.Application.Services;

namespace Neuralm.Services.UserService.Mapping
{
    /// <summary>
    /// Represents the <see cref="UserStartupExtensions"/> class.
    /// </summary>
    public static class UserStartupExtensions
    {
        /// <summary>
        /// Adds services from the <see cref="Application"/> assembly into the <see cref="serviceCollection"/>.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <returns>Returns the service collection to chain further upon.</returns>
        public static IServiceCollection AddApplicationServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddAutoMapper(Assembly.GetAssembly(typeof(UserStartupExtensions)));

            if (StartupExtensions.IsDebug)
                serviceCollection.AddLogging(p => p.AddDebug());

            serviceCollection.AddSingleton<IFactory<UserDbContext>, UserDatabaseFactory>();

            // Instead of using .AddDbContext, .AddTransient is used because, the IFactory<UserDbContext>
            // needs to be used for creating an instance of the UserDbContext.
            serviceCollection.AddTransient<UserDbContext>(p => p.GetService<IFactory<UserDbContext>>().Create());

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
            serviceCollection.AddTransient<IRepository<User>, Repository<User, UserDbContext>>();
            serviceCollection.AddTransient<IRepository<Credential>, Repository<Credential, UserDbContext>>();
            serviceCollection.AddTransient<IRepository<CredentialType>, Repository<CredentialType, UserDbContext>>();
            serviceCollection.AddTransient<IRepository<UserRole>, Repository<UserRole, UserDbContext>>();
            serviceCollection.AddTransient<IRepository<Role>, Repository<Role, UserDbContext>>();
            serviceCollection.AddTransient<IRepository<RolePermission>, Repository<RolePermission, UserDbContext>>();
            serviceCollection.AddTransient<IRepository<Permission>, Repository<Permission, UserDbContext>>();
            #endregion Repositories

            #region Services
            serviceCollection.AddTransient<IAccessTokenService, JwtAccessTokenService>();
            serviceCollection.AddTransient<IUserService, Application.Services.UserService>();
            #endregion Services

            return serviceCollection;
        }
    }
}
