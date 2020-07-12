using Neuralm.Services.Common.Messages.Abstractions;
using Neuralm.Services.Common.Messages.Dtos;
using System;

namespace Neuralm.Services.Common.Messages
{
    /// <summary>
    /// Represents the <see cref="ServiceHealthCheckResponse"/> class.
    /// </summary>
    public class ServiceHealthCheckResponse : Response
    {
        /// <summary>
        /// Gets and sets the service health report.
        /// </summary>
        public ServiceHealthReportDto ServiceHealthReport { get; set; }

        /// <summary>
        /// Initializes an instance of the <see cref="ServiceHealthCheckResponse"/> class.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="serviceHealthReport">The service health report.</param>
        /// <param name="message">The message.</param>
        /// <param name="success">The success flag.</param>
        public ServiceHealthCheckResponse(Guid requestId, ServiceHealthReportDto serviceHealthReport, string message = "", bool success = false) : base(requestId, message, success)
        {
            ServiceHealthReport = serviceHealthReport;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceHealthCheckResponse"/> class.
        /// SERIALIZATION CONSTRUCTOR!
        /// </summary>
        public ServiceHealthCheckResponse()
        {

        }
    }
}
