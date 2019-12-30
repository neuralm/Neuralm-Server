using System.Collections.Generic;
using Neuralm.Services.Common.Messages.Interfaces;

namespace Neuralm.Services.Common.Messages.Abstractions
{
    /// <summary>
    /// Represents the <see cref="PaginationResponse{T}"/> class.
    /// Used for pagination within a REST service.
    /// </summary>
    /// <typeparam name="T">The type to paginate.</typeparam>
    public abstract class PaginationResponse<T> : Response, IPaginationResponse
    {
        /// <summary>
        /// Gets and sets the items.
        /// </summary>
        public List<T> Items { get; set; }
        
        /// <summary>
        /// Gets and sets the page number.
        /// </summary>
        public int PageNumber { get; set; }
        
        /// <summary>
        /// Gets and sets the page size.
        /// </summary>
        public int PageSize { get; set; }
        
        /// <summary>
        /// Gets and sets the total records.
        /// </summary>
        public int TotalRecords { get; set; }
        
        /// <summary>
        /// Gets and sets the page count.
        /// </summary>
        public int PageCount { get; set; }
    }
}