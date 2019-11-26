using Neuralm.Services.Common.Application.Interfaces;

namespace Neuralm.Services.MessageQueue.Application.Interfaces
{
    /// <summary>
    /// Represents the <see cref="IClientMessageTypeCache"/> interface.
    /// Used as a marker interface for the <see cref="IClientMessageProcessor"/> implementation.
    /// </summary>
    public interface IClientMessageTypeCache : IMessageTypeCache
    {
        
    }
}