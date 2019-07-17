using System;

namespace Neuralm.Application.Messages
{
    /// <summary>
    /// The interface for a Response.
    /// </summary>
    public interface IResponse
    {
        Guid Id { get; }
        Guid RequestId { get; }
        DateTime DateTime { get; }
        bool Success { get; }
    }
}