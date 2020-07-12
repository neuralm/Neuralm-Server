using Neuralm.Services.Common.Messages.Abstractions;

namespace Neuralm.Services.Common.Messages
{
    /// <summary>
    /// Represents the <see cref="ServiceHealthCheckRequest"/> class.
    /// Used to perform health checks on services.
    /// </summary>
    [Message("Get", "/health", typeof(ServiceHealthCheckResponse))]
    public class ServiceHealthCheckRequest : Request
    {

    }
}
