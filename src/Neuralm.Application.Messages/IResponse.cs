using System;

namespace Neuralm.Application.Messages
{
    public interface IResponse
    {
        Guid Id { get; }
        Guid RequestId { get; }
    }
}