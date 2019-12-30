namespace Neuralm.Services.Common.Messages.Interfaces
{
    /// <summary>
    /// Represents the <see cref="IPaginationRequest"/> interface.
    /// </summary>
    public interface IPaginationRequest
    {
        /// <summary>
        /// Gets and sets the page number.
        /// </summary>
        int PageNumber { get; set; }
        
        /// <summary>
        /// Gets and sets the page size.
        /// </summary>
        int PageSize { get; set; }
    }
}