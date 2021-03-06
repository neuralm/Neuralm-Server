﻿using Microsoft.Extensions.Logging;
using Neuralm.Services.Common.Application.Interfaces;
using Neuralm.Services.MessageQueue.Application.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Neuralm.Services.MessageQueue.Infrastructure
{
    /// <summary>
    /// Represents the <see cref="MessageToServiceMapper"/> class.
    /// </summary>
    public class MessageToServiceMapper : IMessageToServiceMapper
    {
        private readonly ILogger<MessageToServiceMapper> _logger;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private static readonly ConcurrentDictionary<string, List<Type>> ServiceTypeCache = new ConcurrentDictionary<string, List<Type>>();
        private readonly ConcurrentDictionary<Type, IServiceConnector> _messageToServiceMap = new ConcurrentDictionary<Type, IServiceConnector>();
        private readonly ConcurrentDictionary<Guid, IServiceConnector> _serviceMap = new ConcurrentDictionary<Guid, IServiceConnector>();
        private readonly Regex _regex = new Regex(@"(?:([A-Za-z]*))Service(?=\.)");

        /// <inheritdoc cref="IMessageToServiceMapper.MessageToServiceMap"/>
        public IReadOnlyDictionary<Type, IServiceConnector> MessageToServiceMap => _messageToServiceMap;

        /// <inheritdoc cref="IMessageToServiceMapper.ServiceMap"/>
        public IReadOnlyDictionary<Guid, IServiceConnector> ServiceMap => _serviceMap;

        /// <summary>
        /// Initializes an instance of the <see cref="MessageToServiceMapper"/> class.
        /// </summary>
        /// <param name="messageTypeCache">The message type cache.</param>
        /// <param name="logger">The logger.</param>
        public MessageToServiceMapper(IClientMessageTypeCache messageTypeCache, ILogger<MessageToServiceMapper> logger)
        {
            _logger = logger;
            _logger.LogInformation("Mapping messages to services...");
            foreach (Type type in messageTypeCache.GetMessageTypes())
            {
                string typename = type.FullName ?? "";
                Match match = _regex.Match(typename);
                if (match.Success)
                {
                    ServiceTypeCache.AddOrUpdate(
                        match.Groups[0].Value,
                        value => new List<Type>() { type },
                        (value, list) =>
                        {
                            list.Add(type);
                            return list;
                        });
                }
            }
            _cancellationTokenSource = new CancellationTokenSource();
            _logger.LogInformation("Finished Mapping messages to services!");
        }

        /// <inheritdoc cref="IMessageToServiceMapper.AddService(Guid, string, INetworkConnector)"/>
        public void AddService(Guid id, string name, INetworkConnector networkConnector)
        {
            if (!ServiceTypeCache.ContainsKey(name))
            {
                _logger.LogError("Service is not added because it has no message types associated with it. ");
                return;
            }

            Type type = ServiceTypeCache[name][0];
            if (_messageToServiceMap.ContainsKey(type))
            {
                IServiceConnector serviceConnector = _messageToServiceMap[type];
                _serviceMap.TryAdd(id, serviceConnector);
                serviceConnector.AddService(networkConnector, id);
            }
            else
            {
                IServiceConnector serviceConnector = new ServiceConnector(networkConnector, name, id);
                _serviceMap.TryAdd(id, serviceConnector);
                foreach (Type messageType in ServiceTypeCache[name])
                {
                    _messageToServiceMap.TryAdd(messageType, serviceConnector);
                }
                _ = Task.Run(() => serviceConnector.StartPublishingAsync(_cancellationTokenSource.Token), _cancellationTokenSource.Token);
            }
        }

        /// <inheritdoc cref="IMessageToServiceMapper.RemoveService(Guid)"/>
        public void RemoveService(Guid serviceId)
        {
            _serviceMap.Remove(serviceId, out IServiceConnector serviceConnector);
            serviceConnector.RemoveService(serviceId);
        }
    }
}
