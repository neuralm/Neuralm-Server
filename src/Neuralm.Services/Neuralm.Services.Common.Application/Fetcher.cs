using Neuralm.Services.Common.Application.Interfaces;
using System;

namespace Neuralm.Services.Common.Application
{
    /// <summary>
    /// Represents the <see cref="Fetcher{T}"/> class.
    /// </summary>
    /// <typeparam name="T">The type parameter.</typeparam>
    public class Fetcher<T> : IFetch<T>
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="Fetcher{T}"/> class.
        /// </summary>
        /// <param name="serviceProvider"></param>
        public Fetcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc cref="IFetch{T}.Fetch()"/>
        public T Fetch()
        {
            return (T) _serviceProvider.GetService(typeof(T));
        }
    }
}
