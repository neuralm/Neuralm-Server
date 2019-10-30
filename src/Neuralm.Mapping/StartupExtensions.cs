using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Neuralm.Application.Configurations;
using Neuralm.Application.Cryptography;
using Neuralm.Application.Interfaces;
using Neuralm.Application.Services;
using Neuralm.Application.Validators;
using Neuralm.Domain.Entities;
using Neuralm.Domain.Entities.Authentication;
using Neuralm.Domain.Entities.NEAT;
using Neuralm.Persistence.Contexts;
using Neuralm.Persistence.Infrastructure;
using Neuralm.Persistence.Repositories;
using System;
using System.Text;
using Neuralm.Infrastructure.EndPoints;
using Neuralm.Infrastructure.Interfaces;
using Neuralm.Infrastructure.MessageSerializers;
using Microsoft.OpenApi.Models;

namespace Neuralm.Mapping
{
    /// <summary>
    /// Represents the <see cref="StartupExtensions"/> class.
    /// </summary>
    public static class StartupExtensions
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        /// <summary>
        /// Gets a value whether the application is running in Debug mode.
        /// </summary>
        private static bool IsDebug
        {
            get
            {
                bool isDebug = false;
                #if DEBUG
                isDebug = true;
                #endif
                return isDebug;
            }
        }

        /// <summary>
        /// Adds services from the <see cref="Application.Interfaces"/> assembly into the <see cref="serviceCollection"/>.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <returns>Returns the service collection to chain further upon.</returns>
        public static IServiceCollection AddApplicationServices(this IServiceCollection serviceCollection)
        {
            if (IsDebug)
                serviceCollection.AddLogging(p => p.AddDebug());
            serviceCollection.AddSingleton<IFactory<NeuralmDbContext>, NeuralmDbFactory>();

            // Instead of using .AddDbContext, .AddTransient is used because, the IFactory<NeuralmDbContext>
            // needs to be used for creating an instance of the NeuralmDbContext.
            serviceCollection.AddTransient<NeuralmDbContext>(p => p.GetService<IFactory<NeuralmDbContext>>().Create());

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
            serviceCollection.AddTransient<IEntityValidator<TrainingRoom>, TrainingRoomValidator>();
            serviceCollection.AddTransient<IEntityValidator<TrainingSession>, TrainingSessionValidator>();
            serviceCollection.AddTransient<IEntityValidator<TrainingRoomSettings>, TrainingRoomSettingsValidator>();
            #endregion Validators

            #region Repositories
            // Explicit repository mapping
            serviceCollection.AddTransient<IRepository<TrainingRoom>, TrainingRoomRepository>();
            serviceCollection.AddTransient<IRepository<TrainingSession>, TrainingSessionRepository>();

            // Default repository mapping
            serviceCollection.AddTransient<IRepository<User>, NeuralmDbContextRepository<User>>();
            serviceCollection.AddTransient<IRepository<Credential>, NeuralmDbContextRepository<Credential>>();
            serviceCollection.AddTransient<IRepository<CredentialType>, NeuralmDbContextRepository<CredentialType>>();
            serviceCollection.AddTransient<IRepository<UserRole>, NeuralmDbContextRepository<UserRole>>();
            serviceCollection.AddTransient<IRepository<Role>, NeuralmDbContextRepository<Role>>();
            serviceCollection.AddTransient<IRepository<RolePermission>, NeuralmDbContextRepository<RolePermission>>();
            serviceCollection.AddTransient<IRepository<Permission>, NeuralmDbContextRepository<Permission>>();
            serviceCollection.AddTransient<IRepository<TrainingRoomSettings>, NeuralmDbContextRepository<TrainingRoomSettings>>();
            #endregion Repositories

            #region Services
            serviceCollection.AddSingleton<IAccessTokenService, JwtAccessTokenService>();
            serviceCollection.AddTransient<IUserService, UserService>();
            serviceCollection.AddTransient<ITrainingRoomService, TrainingRoomService>();
            #endregion Services

            #region EndPoints
            serviceCollection.AddSingleton<IClientEndPoint, ClientEndPoint>();
            serviceCollection.AddSingleton<IRestEndPoint, RestEndPoint>();
            #endregion EndPoints

            serviceCollection.AddSingleton<IMessageSerializer, JsonMessageSerializer>();
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
            serviceCollection.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Neuralm", Version = "v1" });
            });
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
