using Neuralm.Services.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Neuralm.Services.Common.Application.Interfaces
{
    /// <summary>
    /// Represents the <see cref="IService{TDto}"/> interface.
    /// </summary>
    public interface IService<TDto> where TDto : class
    {
        /// <summary>
        /// Finds a single dto by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Returns the dto by id.</returns>
        Task<TDto> FindSingleOrDefaultAsync(Guid id);

        /// <summary>
        /// Gets all Entities.
        /// </summary>
        /// <returns>Returns all Entities.</returns>
        Task<IEnumerable<TDto>> GetAllAsync();

        /// <summary>
        /// Deletes the provided dto.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <exception cref="EntityNotFoundException">If the provided dto cannot be found.</exception>
        /// <exception cref="DeletingEntityFailedException">If it fails to the save changes.</exception>
        /// <returns>
        /// Returns (<c>true</c>, <c>true</c>) If the dto is successfully deleted or
        /// (<c>false</c>, <c>true</c>) if the dto was found but not deleted
        ///  otherwise, (<c>false</c>, <c>false</c>) if the delete fails and no dto was found.
        /// </returns>
        Task<(bool success, bool found)> DeleteAsync(TDto dto);

        /// <summary>
        /// Updates the provided dto.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <exception cref="UpdatingEntityFailedException">If it fails to the save changes.</exception>
        /// <returns>
        /// Returns (<c>true</c>, <see cref="Guid"/>, <c>false</c>) If the dto is successfully updated or
        /// (<c>true</c>, <see cref="Guid"/>, <c>true</c>) when instead of updating it created a new dto, otherwise
        /// (<c>false</c>, <see cref="Guid"/>, <c>false</c>) if it fails to update.
        /// </returns>
        Task<(bool success, Guid id, bool updated)> UpdateAsync(TDto dto);

        /// <summary>
        /// Creates the provided dto.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <exception cref="CreatingEntityFailedException">If it fails to the save changes.</exception>
        /// <returns>Returns (<c>true</c>, <see cref="Guid"/>) If the dto is successfully created; otherwise, (<c>false</c>, <see cref="Guid"/>).</returns>
        Task<(bool success, Guid id)> CreateAsync(TDto dto);
    }
}
