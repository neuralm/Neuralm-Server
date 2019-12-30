using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Neuralm.Services.Common.Application.Interfaces;
using Neuralm.Services.Common.Application.Services;
using Neuralm.Services.Common.Mapping;
using Neuralm.Services.Common.Persistence;
using Neuralm.Services.Common.Persistence.EFCore.Repositories;
using Neuralm.Services.RegistryService.Application.Interfaces;
using Neuralm.Services.RegistryService.Domain;
using Neuralm.Services.RegistryService.Persistence.Contexts;
using Neuralm.Services.RegistryService.Persistence.Infrastructure;
using Neuralm.Services.RegistryService.Persistence.Validators;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Neuralm.Services.Common.Application.Serializers;
using Neuralm.Services.Common.Infrastructure;
using Neuralm.Services.Common.Persistence.EFCore;
using Neuralm.Services.RegistryService.Application.Configurations;
using Neuralm.Services.RegistryService.Infrastructure;
using Neuralm.Services.RegistryService.Messages;

namespace Neuralm.Services.RegistryService.Mapping
{
    /// <summary>
    /// Represents the <see cref="RegistryStartupExtensions"/> class.
    /// </summary>
    public static class RegistryStartupExtensions
    {
        /// <summary>
        /// Adds services from the <see cref="Application"/> assembly into the <see cref="serviceCollection"/>.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>Returns the service collection to chain further upon.</returns>
        public static IServiceCollection AddApplicationServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.Configure<NeuralmConfiguration>(configuration.GetSection("Neuralm").Bind);

            serviceCollection.AddAutoMapper(Assembly.GetAssembly(typeof(RegistryStartupExtensions)));
            if (StartupExtensions.IsDebug)
                serviceCollection.AddLogging(p => p.AddDebug());

            serviceCollection.AddSingleton<IFactory<ServiceDbContext>, ServiceDatabaseFactory>();
            serviceCollection.AddSingleton<IFactory<IMessageTypeCache, IEnumerable<Type>>, MessageTypeCacheFactory>();
            serviceCollection.AddSingleton<IMessageTypeCache>(serviceCollection =>
            {
                List<Type> types = new List<Type>()
                {
                    typeof(AddServiceCommand)
                };
                return serviceCollection.GetService<IFactory<IMessageTypeCache, IEnumerable<Type>>>().Create(types);
            });
            
            // Instead of using .AddDbContext, .AddTransient is used because, the IFactory<ServiceDbContext>
            // needs to be used for creating an instance of the ServiceDbContext.
            serviceCollection.AddTransient<ServiceDbContext>(p => p.GetService<IFactory<ServiceDbContext>>().Create());

            #region Validators
            serviceCollection.AddTransient<IEntityValidator<Service>, ServiceValidator>();
            #endregion Validators

            #region Repositories
            serviceCollection.AddTransient<IRepository<Service>, Repository<Service, ServiceDbContext>>();
            #endregion Repositories
            
            serviceCollection.AddTransient<IMessageSerializer, JsonMessageSerializer>();
            serviceCollection.AddTransient<IMessageProcessor, MessageProcessor>();
            
            #region Services
            serviceCollection.AddTransient<IAccessTokenService, JwtAccessTokenService>();
            serviceCollection.AddSingleton<Application.Interfaces.IRegistryService, Infrastructure.Services.RegistryService>();
            #endregion Services
            
            serviceCollection.VerifyDatabaseConnection<ServiceDbContext>();

            return serviceCollection;
        }
        
        /// <summary>
        /// Starts the registry service asynchronously.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <returns>Returns the application builder.</returns>
        public static IApplicationBuilder StartRegistryServiceAsync(this IApplicationBuilder app)
        {
            Application.Interfaces.IRegistryService registryService = app.ApplicationServices.GetService(typeof(Application.Interfaces.IRegistryService)) as Application.Interfaces.IRegistryService;
            Task.Run(async () => await registryService.StartupAsync(CancellationToken.None));
            return app;
        }
    }
}
