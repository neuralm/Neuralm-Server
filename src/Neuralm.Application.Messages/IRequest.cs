using System;

namespace Neuralm.Application.Messages
{
    /// <summary>
    /// The interface for a Request.
    /// </summary>
    public interface IRequest
    {
        Guid Id { get; }
        DateTime DateTime { get; }
    }
}
