using Neuralm.Services.Common.Messages.Abstractions;
using Neuralm.Services.Common.Messages.Interfaces;

namespace Neuralm.Services.Common.Messages
{
    /// <summary>
    /// Represents the <see cref="PaginationRequest"/> class.
    /// Used for pagination within a REST service.
    /// </summary>
    public abstract class PaginationRequest : Request, IPaginationRequest
    {
        /// <summary>
        /// Gets and sets the page number.
        /// </summary>
        public int PageNumber { get; set; }
        
        /// <summary>
        /// Gets and sets the page size.
        /// </summary>
        public int PageSize { get; set; }
    }
}