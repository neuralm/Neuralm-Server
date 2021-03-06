﻿using Microsoft.Extensions.Logging;
using Neuralm.Services.Common.Application.Interfaces;
using Neuralm.Services.Common.Messages.Interfaces;
using Neuralm.Services.MessageQueue.Application.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using IRegistryService = Neuralm.Services.MessageQueue.Application.Interfaces.IRegistryService;

namespace Neuralm.Services.MessageQueue.Infrastructure
{
    /// <summary>
    /// Represents the <see cref="RegistryServiceMessageProcessor"/> class.
    /// </summary>
    public class RegistryServiceMessageProcessor : IRegistryServiceMessageProcessor
    {
        private readonly ConcurrentDictionary<Type, MethodInfo> _messageToMethodMap = new ConcurrentDictionary<Type, MethodInfo>();
        private readonly IRegistryService _registryService;
        private readonly ILogger<RegistryServiceMessageProcessor> _logger;

        /// <summary>
        /// Initializes an instance of the <see cref="RegistryServiceMessageProcessor"/> class.
        /// </summary>
        /// <param name="registryService">The registry service fetcher.</param>
        /// <param name="logger">The logger.</param>
        public RegistryServiceMessageProcessor(IRegistryService registryService, ILogger<RegistryServiceMessageProcessor> logger)
        {
            _registryService = registryService;
            _logger = logger;
            Type serviceType = registryService.GetType();
            foreach ((MethodInfo methodInfo, Type parameterType) in serviceType
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.IsFinal && p.GetParameters()[0].ParameterType != typeof(CancellationToken))
                .Select(methodInfo => (methodInfo, parameterType: methodInfo.GetParameters()[0].ParameterType)))
            {
                _messageToMethodMap.TryAdd(parameterType, methodInfo);
                Console.WriteLine($"\t {parameterType.Name} -> {serviceType.Name}.{methodInfo.Name}");
            }
        }

        /// <inheritdoc cref="IMessageProcessor.ProcessMessageAsync(IMessage, INetworkConnector)"/>
        public Task ProcessMessageAsync(IMessage message, INetworkConnector networkConnector)
        {
            _logger.LogInformation($"Started processing RegistryService({networkConnector.EndPoint}) message.");
            if (!_messageToMethodMap.TryGetValue(message.GetType(), out MethodInfo methodInfo))
                throw new ArgumentOutOfRangeException(nameof(message), $"Unknown Request message of type: {message.GetType().FullName}");
            return InvokeMethodAsync(message, methodInfo)
                .ContinueWith((task) =>
                    {
                        _logger.LogInformation($"Finished processing RegistryService({networkConnector.EndPoint}) message {(task.IsCompletedSuccessfully ? "successfully" : "unsuccessfully.")}.");
                        return task;
                    });
        }

        private async Task InvokeMethodAsync(IMessage message, MethodInfo methodInfo)
        {
            dynamic task = methodInfo.Invoke(_registryService, new object[] { message });
            await task;
        }
    }
}
