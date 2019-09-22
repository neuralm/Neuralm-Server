using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Neuralm.Services.Common.Configurations;

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
        /// <returns>Returns the service collection.</returns>
        public static IServiceCollection AddAutoMapper(this IServiceCollection serviceCollection, Assembly assembly)
        {
            serviceCollection.AddAutoMapper(new [] { assembly });
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
