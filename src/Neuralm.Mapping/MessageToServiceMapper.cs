using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Neuralm.Application.Interfaces;
using MethodInfo = System.Reflection.MethodInfo;

namespace Neuralm.Mapping
{
    /// <summary>
    /// Represents the <see cref="MessageToServiceMapper"/> class.
    /// </summary>
    public class MessageToServiceMapper
    {
        private readonly ConcurrentDictionary<Type, (object, MethodInfo)> _messageToServiceMap = new ConcurrentDictionary<Type, (object, MethodInfo)>();
        private readonly ConcurrentDictionary<Type, object> _services = new ConcurrentDictionary<Type, object>();

        /// <summary>
        /// Gets the message to service map dictionary.
        /// </summary>
        public IReadOnlyDictionary<Type, (object, MethodInfo)> MessageToServiceMap => _messageToServiceMap;

        /// <summary>
        /// Initializes an instance of the <see cref="MessageToServiceMapper"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public MessageToServiceMapper(IServiceProvider serviceProvider)
        {
            Console.WriteLine("Mapping messages to services...");
            List<(Type serviceType, MethodInfo methodInfo, Type parameterType)> x = typeof(IService)
                .Assembly
                .GetTypes()
                .Where(type => type.GetInterfaces().Contains(typeof(IService)) && type.IsClass)
                .Select(type =>
                    (serviceType: type,
                        serviceMethods: type
                            .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                            .Where(method => method.IsFinal)))
                .SelectMany(tuple => tuple.serviceMethods.Select(methodInfo => (tuple.serviceType.GetInterfaces()[0], methodInfo,
                    parameterType: methodInfo.GetParameters()[0].ParameterType)))
                .ToList();
            foreach ((Type serviceType, MethodInfo methodInfo, Type parameterType) in x)
            {
                if (!_services.TryGetValue(serviceType, out object service))
                {
                    service = serviceProvider.GetService(serviceType);
                    _services.TryAdd(serviceType, service);
                }

                _messageToServiceMap.TryAdd(parameterType, (service, methodInfo));
                Console.WriteLine($"\t {parameterType.Name} -> {serviceType.Name}.{methodInfo.Name}");
            }
            Console.WriteLine("Finished Mapping messages to services!");
        }
    }
}
