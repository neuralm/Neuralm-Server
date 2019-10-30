using Neuralm.Services.Common.Messages.Interfaces;
using Neuralm.Services.MessageQueue.Application.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Neuralm.Services.Common.Application.Interfaces;

namespace Neuralm.Services.MessageQueue.Infrastructure
{
    /// <summary>
    /// Represents the <see cref="RegistryServiceMessageProcessor"/> class.
    /// </summary>
    public class RegistryServiceMessageProcessor : IRegistryServiceMessageProcessor
    {
        private readonly ConcurrentDictionary<Type, MethodInfo> _messageToMethodMap = new ConcurrentDictionary<Type, MethodInfo>();
        private readonly IFetch<IRegistryService> _registryServiceFetcher;

        /// <summary>
        /// Initializes an instance of the <see cref="RegistryServiceMessageProcessor"/> class.
        /// </summary>
        /// <param name="registryServiceFetcher">The registry service fetcher.</param>
        public RegistryServiceMessageProcessor(IFetch<IRegistryService> registryServiceFetcher)
        {
            _registryServiceFetcher = registryServiceFetcher;
            Type serviceType = registryServiceFetcher.GetType().GetGenericArguments()[0];
            foreach ((MethodInfo methodInfo, Type parameterType) in serviceType
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.IsFinal)
                .Select(methodInfo => (methodInfo, parameterType: methodInfo.GetParameters()[0].ParameterType)))
            {
                _messageToMethodMap.TryAdd(parameterType, methodInfo);
                Console.WriteLine($"\t {parameterType.Name} -> {serviceType.Name}.{methodInfo.Name}");
            }
        }

        /// <inheritdoc cref="IMessageProcessor.ProcessMessageAsync(IMessage, INetworkConnector)"/>
        public async Task ProcessMessageAsync(IMessage message, INetworkConnector networkConnector)
        {
            Console.WriteLine($"Started processing RegistryService({networkConnector.EndPoint}) message.");
            if (!_messageToMethodMap.TryGetValue(message.GetType(), out MethodInfo methodInfo))
                throw new ArgumentOutOfRangeException(nameof(message), $"Unknown Request message of type: {message.GetType().Name}");
            dynamic task = methodInfo.Invoke(_registryServiceFetcher.Fetch(), new object[] {message});
            await task;
            Console.WriteLine($"Finished processing RegistryService({networkConnector.EndPoint}) message.");
        }
    }
}
