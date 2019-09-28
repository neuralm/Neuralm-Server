using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Neuralm.Services.Common.Application.Interfaces;
using Neuralm.Services.Common.Mapping;
using Neuralm.Services.Common.Messaging.Serializers;
using Neuralm.Services.Common.Persistence;
using Neuralm.Services.Common.Persistence.EFCore.Repositories;
using Neuralm.Services.TrainingRoomService.Application.Interfaces;
using Neuralm.Services.TrainingRoomService.Domain;
using Neuralm.Services.TrainingRoomService.Persistence.Contexts;
using Neuralm.Services.TrainingRoomService.Persistence.Infrastructure;
using Neuralm.Services.TrainingRoomService.Persistence.Validators;
using System.Reflection;
using Neuralm.Services.Common.Application.Services;

namespace Neuralm.Services.TrainingRoomService.Mapping
{
    /// <summary>
    /// Represents the <see cref="TrainingRoomStartupExtensions"/> class.
    /// </summary>
    public static class TrainingRoomStartupExtensions
    {
        /// <summary>
        /// Adds services from the <see cref="Application"/> assembly into the <see cref="serviceCollection"/>.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <returns>Returns the service collection to chain further upon.</returns>
        public static IServiceCollection AddApplicationServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddAutoMapper(Assembly.GetAssembly(typeof(TrainingRoomStartupExtensions)));

            if (StartupExtensions.IsDebug)
                serviceCollection.AddLogging(p => p.AddDebug());

            serviceCollection.AddSingleton<IFactory<TrainingRoomDbContext>, TrainingRoomDatabaseFactory>();

            // Instead of using .AddDbContext, .AddTransient is used because, the IFactory<TrainingRoomDbContext>
            // needs to be used for creating an instance of the TrainingRoomDbContext.
            serviceCollection.AddTransient<TrainingRoomDbContext>(p => p.GetService<IFactory<TrainingRoomDbContext>>().Create());

            #region Validators
            serviceCollection.AddTransient<IEntityValidator<TrainingRoom>, TrainingRoomValidator>();
            serviceCollection.AddTransient<IEntityValidator<TrainingSession>, TrainingSessionValidator>();
            serviceCollection.AddTransient<IEntityValidator<TrainingRoomSettings>, TrainingRoomSettingsValidator>();
            #endregion Validators

            #region Repositories
            serviceCollection.AddTransient<IRepository<TrainingRoom>, Repository<TrainingRoom, TrainingRoomDbContext>>();
            serviceCollection.AddTransient<IRepository<TrainingSession>, Repository<TrainingSession, TrainingRoomDbContext>>();
            serviceCollection.AddTransient<IRepository<TrainingRoomSettings>, Repository<TrainingRoomSettings, TrainingRoomDbContext>>();
            #endregion Repositories

            #region Services
            serviceCollection.AddSingleton<ITrainingRoomService, Application.Services.TrainingRoomService>();
            serviceCollection.AddSingleton<ITrainingSessionService, Application.Services.TrainingSessionService>();
            serviceCollection.AddTransient<IAccessTokenService, JwtAccessTokenService>();
            #endregion Services

            serviceCollection.AddSingleton<IMessageSerializer, JsonMessageSerializer>();

            return serviceCollection;
        }
    }
}
