using Neuralm.Services.Common.Messages.Interfaces;
using Neuralm.Services.MessageQueue.Application.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Neuralm.Services.MessageQueue.Infrastructure
{
    /// <summary>
    /// Represents the <see cref="RegistryServiceMessageProcessor"/> class.
    /// </summary>
    public class RegistryServiceMessageProcessor : IRegistryServiceMessageProcessor
    {
        private readonly ConcurrentDictionary<Type, MethodInfo> _messageToMethodMap = new ConcurrentDictionary<Type, MethodInfo>();
        private readonly IRegistryService _registryService;

        /// <summary>
        /// Initializes an instance of the <see cref="RegistryServiceMessageProcessor"/> class.
        /// </summary>
        /// <param name="registryService">The registry service.</param>
        public RegistryServiceMessageProcessor(IRegistryService registryService)
        {
            _registryService = registryService;
            Type serviceType = registryService.GetType();
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
            if (_messageToMethodMap.TryGetValue(message.GetType(), out MethodInfo methodInfo))
            {
                dynamic task = methodInfo.Invoke(_registryService, new object[] { message });
                await task;
            }
            else
                throw new ArgumentOutOfRangeException(nameof(message), $"Unknown Request message of type: {message.GetType().Name}");
            Console.WriteLine($"Finished processing RegistryService({networkConnector.EndPoint}) message.");
        }
    }
}
