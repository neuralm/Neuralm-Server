namespace Neuralm.Services.Common.Messages.Interfaces
{
    /// <summary>
    /// Represents the <see cref="IPaginationResponse"/> interface.
    /// </summary>
    public interface IPaginationResponse : IResponseMessage
    {
        /// <summary>
        /// Gets and sets the page number.
        /// </summary>
        int PageNumber { get; set; }
        
        /// <summary>
        /// Gets and sets the page size.
        /// </summary>
        int PageSize { get; set; }
        
        /// <summary>
        /// Gets and sets the total records.
        /// </summary>
        int TotalRecords { get; set; }
        
        /// <summary>
        /// Gets and sets the page count.
        /// </summary>
        int PageCount { get; set; }
    }
}