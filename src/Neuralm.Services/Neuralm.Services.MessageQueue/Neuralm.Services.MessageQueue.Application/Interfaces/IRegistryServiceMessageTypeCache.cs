using Neuralm.Services.Common.Application.Interfaces;

namespace Neuralm.Services.MessageQueue.Application.Interfaces
{
    /// <summary>
    /// Represents the <see cref="IRegistryServiceMessageTypeCache"/> interface.
    /// Used as a marker interface for the <see cref="IRegistryService"/> implementation.
    /// </summary>
    public interface IRegistryServiceMessageTypeCache : IMessageTypeCache
    {
        
    }
}