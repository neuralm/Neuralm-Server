﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Neuralm.Services.Common.Application.Interfaces;
using Neuralm.Services.Common.Application.Serializers;
using Neuralm.Services.Common.Application.Services;
using Neuralm.Services.Common.Infrastructure;
using Neuralm.Services.Common.Mapping;
using Neuralm.Services.Common.Messages;
using Neuralm.Services.Common.Patterns;
using Neuralm.Services.MessageQueue.Application;
using Neuralm.Services.MessageQueue.Application.Interfaces;
using Neuralm.Services.MessageQueue.Infrastructure;
using Neuralm.Services.RegistryService.Messages;
using Neuralm.Services.TrainingRoomService.Messages;
using Neuralm.Services.UserService.Messages;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Neuralm.Services.MessageQueue.Mapping
{
    /// <summary>
    /// Represents the <see cref="MessageQueueStartupExtensions"/> class.
    /// </summary>
    public static class MessageQueueStartupExtensions
    {
        /// <summary>
        /// Adds services from the <see cref="Application"/> assembly into the <see cref="serviceCollection"/>.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <returns>Returns the service collection to chain further upon.</returns>
        public static IServiceCollection AddApplicationServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddAutoMapper(Assembly.GetAssembly(typeof(MessageQueueStartupExtensions)));

            serviceCollection.AddLogging(p => p.AddConsole());

            serviceCollection.AddSingleton<IFactory<IMessageTypeCache, IEnumerable<Type>>, MessageTypeCacheFactory>();
            serviceCollection.AddSingleton<IClientMessageTypeCache, ClientMessageTypeCache>(serviceCollection =>
            {
                List<Type> types = new List<Type>
                {
                    typeof(RegisterRequest), typeof(CreateTrainingRoomRequest), typeof(ServiceHealthCheckRequest)
                };
                IMessageTypeCache messageTypeCache = serviceCollection.GetService<IFactory<IMessageTypeCache, IEnumerable<Type>>>().Create(types);
                return new ClientMessageTypeCache(messageTypeCache);
            });
            
            serviceCollection.AddSingleton<IRegistryServiceMessageTypeCache, RegistryServiceMessageTypeCache>(serviceCollection =>
            {
                List<Type> types = new List<Type>
                {
                    typeof(AddServiceCommand)
                };
                IMessageTypeCache messageTypeCache = serviceCollection.GetService<IFactory<IMessageTypeCache, IEnumerable<Type>>>().Create(types);
                return new RegistryServiceMessageTypeCache(messageTypeCache);
            });
            serviceCollection.AddSingleton<IMessageSerializer, JsonMessageSerializer>();
            serviceCollection.AddSingleton<IAccessTokenService, JwtAccessTokenService>();
            serviceCollection.AddSingleton<IMessageToServiceMapper, MessageToServiceMapper>();
            serviceCollection.AddSingleton<IServiceMessageProcessor, ServiceMessageProcessor>();
            serviceCollection.AddSingleton<IClientMessageProcessor, ClientMessageProcessor>();

            #region Services
            serviceCollection.AddSingleton<Application.Interfaces.IRegistryService, Infrastructure.Services.RegistryService>();
            #endregion Services
            
            return serviceCollection;
        }
    }
}
