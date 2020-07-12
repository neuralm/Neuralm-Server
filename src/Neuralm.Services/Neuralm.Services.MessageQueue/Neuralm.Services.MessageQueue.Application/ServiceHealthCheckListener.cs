using Neuralm.Services.Common.Application;
using Neuralm.Services.Common.Messages;
using System;

namespace Neuralm.Services.MessageQueue.Application
{
    /// <summary>
    /// Represents the <see cref="ServiceHealthCheckListener"/> class.
    /// </summary>
    public class ServiceHealthCheckListener : MessageListener<ServiceHealthCheckResponse>
    {
        /// <inheritdoc cref="MessageListener.OnError"/>
        public override void OnError() => throw new NotImplementedException();
    }
}
