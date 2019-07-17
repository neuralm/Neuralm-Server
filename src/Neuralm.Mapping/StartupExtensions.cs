using System;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Neuralm.Domain.Entities.NEAT;

namespace Neuralm.Mapping
{
    /// <summary>
    /// Represents the <see cref="StartupExtensions"/> class.
    /// </summary>
    public static class StartupExtensions
    {
        /// <summary>
        /// Adds services from the <see cref="Neuralm.Application.Interfaces"/> assembly into the <see cref="serviceCollection"/>.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <returns>Returns the service collection to chain further upon.</returns>
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
            serviceCollection.AddTransient<IEntityValidator<Brain>, BrainValidator>();
            serviceCollection.AddTransient<IEntityValidator<TrainingRoom>, TrainingRoomValidator>();
            serviceCollection.AddTransient<IEntityValidator<TrainingSession>, TrainingSessionValidator>();
            serviceCollection.AddTransient<IEntityValidator<TrainingRoomSettings>, TrainingRoomSettingsValidator>();
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
            serviceCollection.AddTransient<IRepository<Brain>, BrainRepository>(serviceProvider =>
            {
                IFactory<NeuralmDbContext> neuralmDbFactory = serviceProvider.GetService<IFactory<NeuralmDbContext>>();
                IEntityValidator<Brain> entityValidator = serviceProvider.GetService<IEntityValidator<Brain>>();
                return new BrainRepository(neuralmDbFactory.Create(), entityValidator);
            });
            serviceCollection.AddTransient<IRepository<TrainingRoom>, TrainingRoomRepository>(serviceProvider =>
            {
                IFactory<NeuralmDbContext> neuralmDbFactory = serviceProvider.GetService<IFactory<NeuralmDbContext>>();
                IEntityValidator<TrainingRoom> entityValidator = serviceProvider.GetService<IEntityValidator<TrainingRoom>>();
                return new TrainingRoomRepository(neuralmDbFactory.Create(), entityValidator);
            });
            serviceCollection.AddTransient<IRepository<TrainingSession>, TrainingSessionRepository>(serviceProvider =>
            {
                IFactory<NeuralmDbContext> neuralmDbFactory = serviceProvider.GetService<IFactory<NeuralmDbContext>>();
                IEntityValidator<TrainingSession> entityValidator = serviceProvider.GetService<IEntityValidator<TrainingSession>>();
                return new TrainingSessionRepository(neuralmDbFactory.Create(), entityValidator);
            });
            serviceCollection.AddTransient<IRepository<TrainingRoomSettings>, TrainingRoomSettingsRepository>(serviceProvider =>
            {
                IFactory<NeuralmDbContext> neuralmDbFactory = serviceProvider.GetService<IFactory<NeuralmDbContext>>();
                IEntityValidator<TrainingRoomSettings> entityValidator = serviceProvider.GetService<IEntityValidator<TrainingRoomSettings>>();
                return new TrainingRoomSettingsRepository(neuralmDbFactory.Create(), entityValidator);
            });
            #endregion Repositories

            #region Services
            serviceCollection.AddSingleton<IAccessTokenService, JwtAccessTokenService>();
            serviceCollection.AddTransient<IUserService, UserService>();
            serviceCollection.AddTransient<ITrainingRoomService, TrainingRoomService>();
            #endregion Services

            serviceCollection.AddSingleton(p => new MessageToServiceMapper(p));
            return serviceCollection;
        }

        /// <summary>
        /// Adds the Jwt bearer based authentication into the <see cref="serviceCollection"/>.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <returns>Returns the service collection to chain further upon.</returns>
        public static IServiceCollection AddJwtBearerBasedAuthentication(this IServiceCollection serviceCollection)
        {
            JwtConfiguration jwtConfiguration = serviceCollection.BuildServiceProvider().GetService<IOptions<JwtConfiguration>>().Value;
            serviceCollection.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.Secret)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            return serviceCollection;
        }

        /// <summary>
        /// Adds and binds configurations into the <see cref="serviceCollection"/>.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>Returns the service collection to chain further upon.</returns>
        public static IServiceCollection AddConfigurations(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddOptions();
            serviceCollection.Configure<ServerConfiguration>(configuration.GetSection("Server").Bind);
            serviceCollection.Configure<DbConfiguration>(configuration.GetSection("NeuralmDb").Bind);
            serviceCollection.Configure<JwtConfiguration>(configuration.GetSection("Jwt").Bind);
            return serviceCollection;
        }

        /// <summary>
        /// Converts an <see cref="IServiceProvider"/> to an <see cref="IGenericServiceProvider"/>.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns>Returns a <see cref="IGenericServiceProvider"/> implementation.</returns>
        public static IGenericServiceProvider ToGenericServiceProvider(this IServiceProvider serviceProvider)
        {
            return new GenericServiceProvider(serviceProvider);
        }
    }
}
