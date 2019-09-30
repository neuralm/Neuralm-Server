using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Neuralm.Services.Common.Mapping;
using Neuralm.Services.MessageQueue.Application.Interfaces;
using Neuralm.Services.MessageQueue.Application.Serializers;
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

            if (StartupExtensions.IsDebug)
                serviceCollection.AddLogging(p => p.AddDebug());

            //serviceCollection.AddSingleton<IFactory<MessageDbContext>, MessageDatabaseFactory>();

            // Instead of using .AddDbContext, .AddTransient is used because, the IFactory<MessageDbContext>
            // needs to be used for creating an instance of the UserDbContext.
            //serviceCollection.AddTransient<MessageDbContext>(p => p.GetService<IFactory<MessageDbContext>>().Create());

            serviceCollection.AddSingleton<IMessageQueue, Infrastructure.Messaging.MessageQueue>();

            #region Services
            serviceCollection.AddSingleton<IRegistryService, IRegistryService>();
            #endregion Services

            serviceCollection.AddSingleton<IMessageSerializer, JsonMessageSerializer>();

            return serviceCollection;
        }
    }
}
