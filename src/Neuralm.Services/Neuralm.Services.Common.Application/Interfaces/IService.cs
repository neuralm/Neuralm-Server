using Neuralm.Services.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Neuralm.Services.Common.Application.Interfaces
{
    /// <summary>
    /// Represents the <see cref="IService{TEntity}"/> interface.
    /// </summary>
    public interface IService<TEntity> where TEntity : class
    {
        /// <summary>
        /// Finds a single Entity by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Returns the Entity by id.</returns>
        Task<TEntity> FindSingleOrDefaultAsync(Guid id);

        /// <summary>
        /// Gets all Entities.
        /// </summary>
        /// <returns>Returns all Entities.</returns>
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// Deletes the provided Entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <exception cref="EntityNotFoundException">If the provided Entity cannot be found.</exception>
        /// <exception cref="DeletingEntityFailedException">If it fails to the save changes.</exception>
        /// <returns>
        /// Returns (<c>true</c>, <c>true</c>) If the Entity is successfully deleted or
        /// (<c>false</c>, <c>true</c>) if the entity was found but not deleted
        ///  otherwise, (<c>false</c>, <c>false</c>) if the delete fails and no entity was found.
        /// </returns>
        Task<(bool success, bool found)> DeleteAsync(TEntity entity);

        /// <summary>
        /// Updates the provided Entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <exception cref="UpdatingEntityFailedException">If it fails to the save changes.</exception>
        /// <returns>
        /// Returns (<c>true</c>, <see cref="Guid"/>, <c>false</c>) If the Entity is successfully updated or
        /// (<c>true</c>, <see cref="Guid"/>, <c>true</c>) when instead of updating it created a new entity, otherwise
        /// (<c>false</c>, <see cref="Guid"/>, <c>false</c>) if it fails to update.
        /// </returns>
        Task<(bool success, Guid id, bool updated)> UpdateAsync(TEntity entity);

        /// <summary>
        /// Creates the provided Entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <exception cref="CreatingEntityFailedException">If it fails to the save changes.</exception>
        /// <returns>Returns (<c>true</c>, <see cref="Guid"/>) If the Entity is successfully created; otherwise, (<c>false</c>, <see cref="Guid"/>).</returns>
        Task<(bool success, Guid id)> CreateAsync(TEntity entity);
    }
}
