using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Neuralm.Services.Common.Configurations;
using System;
using System.Reflection;

namespace Neuralm.Services.Common.Mapping
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
        public static bool IsDebug
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
        /// Adds the AutoMapper mappings from the provided assembly.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <param name="assembly">The assembly.</param>
        /// <returns>Returns the service collection to chain further upon.</returns>
        public static IServiceCollection AddAutoMapper(this IServiceCollection serviceCollection, Assembly assembly)
        {
            serviceCollection.AddAutoMapper(new[] { assembly });
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
            serviceCollection.Configure<DbConfiguration>(configuration.GetSection("Database").Bind);
            serviceCollection.Configure<JwtConfiguration>(configuration.GetSection("Jwt").Bind);
            serviceCollection.Configure<RegistryServiceConfiguration>(configuration.GetSection("RegistryService").Bind);
            return serviceCollection;
        }

        /// <summary>
        /// Adds authentication using Jwt bearers.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <param name="jwtConfiguration">The jwt configuration.</param>
        /// <returns>Returns the service collection to chain further upon.</returns>
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection serviceCollection, JwtConfiguration jwtConfiguration)
        {
            serviceCollection.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(jwtConfiguration.SecretBytes),
                    ValidIssuer = jwtConfiguration.Issuer,
                    ValidAudience = jwtConfiguration.Audience,
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            return serviceCollection;
        }

        /// <summary>
        /// Adds Jwt authentication with cors.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <returns>Returns the application builder.</returns>
        public static IApplicationBuilder UseJwtAuthenticationWithCors(this IApplicationBuilder app)
        {
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseAuthentication();
            return app;
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
