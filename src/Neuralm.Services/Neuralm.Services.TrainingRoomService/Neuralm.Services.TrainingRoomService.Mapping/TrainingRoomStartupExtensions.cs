﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Neuralm.Services.Common.Application.Interfaces;
using Neuralm.Services.Common.Application.Serializers;
using Neuralm.Services.Common.Application.Services;
using Neuralm.Services.Common.Exceptions;
using Neuralm.Services.Common.Infrastructure.Services;
using Neuralm.Services.Common.Mapping;
using Neuralm.Services.Common.Messages.Dtos;
using Neuralm.Services.Common.Patterns;
using Neuralm.Services.Common.Persistence;
using Neuralm.Services.Common.Persistence.EFCore;
using Neuralm.Services.Common.Persistence.EFCore.Repositories;
using Neuralm.Services.TrainingRoomService.Application.Interfaces;
using Neuralm.Services.TrainingRoomService.Domain;
using Neuralm.Services.TrainingRoomService.Infrastructure.Services;
using Neuralm.Services.TrainingRoomService.Persistence.Contexts;
using Neuralm.Services.TrainingRoomService.Persistence.Infrastructure;
using Neuralm.Services.TrainingRoomService.Persistence.Repositories;
using Neuralm.Services.TrainingRoomService.Persistence.Validators;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Security.Claims;

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

            serviceCollection.AddLogging(p => p.AddConsole());

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
            serviceCollection.AddTransient<ITrainingSessionRepository, TrainingSessionRepository>();
            serviceCollection.AddTransient<IRepository<TrainingRoomSettings>, Repository<TrainingRoomSettings, TrainingRoomDbContext>>();
            #endregion Repositories
            
            serviceCollection.AddSingleton<IMessageSerializer, JsonMessageSerializer>();

            #region Services
            serviceCollection.AddTransient<IAccessTokenService, JwtAccessTokenService>();
            serviceCollection.AddTransient<ITrainingRoomService, Application.Services.TrainingRoomService>();
            serviceCollection.AddTransient<ITrainingSessionService, Application.Services.TrainingSessionService>();
            serviceCollection.AddRegistryService("TrainingRoomService");
            serviceCollection.AddSingleton<IUserService, UserService>(provider =>
            {
                ILogger<UserService> logger = provider.GetRequiredService<ILogger<UserService>>();
                IMessageSerializer messageSerializer = provider.GetService<IMessageSerializer>();
                HttpClient httpClient = provider.GetService<IHttpClientFactory>().CreateClient("UserService");
                IAccessTokenService accessTokenService = provider.GetService<IAccessTokenService>();
                IRegistryService registryService = provider.GetService<IRegistryService>();
                // NOTE: May deadlock
                ServiceDto serviceDto = registryService.GetServiceAsync("UserService").GetAwaiter().GetResult();
                // NOTE: What if null? maybe wait before user service is available? several attempts?
                if (serviceDto is null)
                {
                    logger.LogError($"Failed to initialize UserService!");
                    throw new InitializationException("Failed to initialize UserService!");
                }
                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "TrainingRoomService"),
                    new Claim(ClaimTypes.Role, "Service")
                };
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessTokenService.GenerateAccessToken(claims)}");
                httpClient.BaseAddress = new Uri($"http://{serviceDto.Host}:{serviceDto.Port}");
                return new UserService(messageSerializer, httpClient, logger);
            });
            serviceCollection.AddSingleton<IStartupService, StartupService>();
            #endregion Services

            serviceCollection.VerifyDatabaseConnection<TrainingRoomDbContext>();

            serviceCollection.AddHealthChecks<TrainingRoomDbContext>();

            return serviceCollection;
        }
    }
}
