using System;

namespace Neuralm.Application.Messages
{
    public interface IResponse
    {
        Guid Id { get; }
        Guid RequestId { get; }
        DateTime DateTime { get; }
        bool Success { get; }
    }
}