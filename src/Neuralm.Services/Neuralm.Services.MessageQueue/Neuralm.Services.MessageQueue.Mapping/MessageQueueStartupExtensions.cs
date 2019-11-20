using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Neuralm.Services.Common.Mapping;
using Neuralm.Services.MessageQueue.Application.Interfaces;
using Neuralm.Services.MessageQueue.Infrastructure;
using System.Reflection;
using Neuralm.Services.Common.Application;
using Neuralm.Services.Common.Application.Interfaces;
using Neuralm.Services.Common.Application.Serializers;
using Neuralm.Services.Common.Infrastructure;
using Neuralm.Services.MessageQueue.Application;
using Neuralm.Services.TrainingRoomService.Messages;
using Neuralm.Services.UserService.Messages;

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

            if (StartupExtensions.IsDebug)
                serviceCollection.AddLogging(p => p.AddDebug());

            // Instead of using .AddDbContext, .AddTransient is used because, the IFactory<MessageDbContext>
            // needs to be used for creating an instance of the UserDbContext.
            //serviceCollection.AddTransient<MessageDbContext>(p => p.GetService<IFactory<MessageDbContext>>().Create());
            
            serviceCollection.AddSingleton<IFactory<IMessageTypeCache, IEnumerable<Type>>, MessageTypeCacheFactory>();
            serviceCollection.AddSingleton<IMessageTypeCache>(serviceCollection =>
            {
                List<Type> types = new List<Type>
                {
                    typeof(RegisterRequest), typeof(CreateTrainingRoomRequest)
                };
                return serviceCollection.GetService<IFactory<IMessageTypeCache, IEnumerable<Type>>>().Create(types);
            });
            serviceCollection.AddSingleton<IMessageSerializer, JsonMessageSerializer>();
            
            serviceCollection.AddSingleton<IMessageToServiceMapper, MessageToServiceMapper>();
            serviceCollection.AddSingleton<IFetch<IRegistryService>, Fetcher<IRegistryService>>();

            #region Services
            serviceCollection.AddSingleton<IRegistryService, Infrastructure.Services.RegistryService>();
            #endregion Services

            serviceCollection.AddSingleton<IRegistryServiceMessageProcessor, RegistryServiceMessageProcessor>();
            serviceCollection.AddSingleton<IServiceMessageProcessor, ServiceMessageProcessor>();
            serviceCollection.AddSingleton<IClientMessageProcessor, ClientMessageProcessor>();

            return serviceCollection;
        }
    }
}
