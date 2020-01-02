﻿using Neuralm.Services.Common.Domain;
using Neuralm.Services.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Neuralm.Services.Common.Application.Interfaces
{
    /// <summary>
    /// Represents the <see cref="IRepository{TEntity}"/> interface for generic repositories.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        /// <summary>
        /// Finds a single Entity by an expression asynchronously.
        /// </summary>
        /// <param name="predicate">The expression.</param>
        /// <returns>Returns the result of the expression.</returns>
        Task<TEntity> FindSingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Finds many Entities using an expression asynchronously.
        /// </summary>
        /// <param name="predicate">The expression.</param>
        /// <returns>The results of the expression.</returns>
        Task<IEnumerable<TEntity>> FindManyAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Gets all Entities asynchronously.
        /// </summary>
        /// <returns>Returns all Entities.</returns>
        Task<IEnumerable<TEntity>> GetAllAsync();
        
        /// <summary>
        /// Gets pagination asynchronously.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>Returns all paginated entities.</returns>
        Task<IEnumerable<TEntity>> GetPaginationAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Counts the Entities asynchronously.
        /// </summary>
        /// <returns>Returns the number of Entities.</returns>
        Task<int> CountAsync();
        
        /// <summary>
        /// Deletes the provided Entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <exception cref="EntityNotFoundException">If the provided Entity cannot be found.</exception>
        /// <exception cref="DeletingEntityFailedException">If it fails to the save changes.</exception>
        /// <returns>Returns <c>true</c> If the Entity is successfully deleted; otherwise, <c>false</c>.</returns>
        Task<bool> DeleteAsync(TEntity entity);

        /// <summary>
        /// Updates the provided Entity asynchronously.
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
        /// Saves the provided Entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <exception cref="CreatingEntityFailedException">If it fails to the save changes.</exception>
        /// <returns>Returns (<c>true</c>, <see cref="Guid"/>) If the Entity is successfully created; otherwise, (<c>false</c>, <see cref="Guid"/>).</returns>
        Task<(bool success, Guid id)> CreateAsync(TEntity entity);

        /// <summary>
        /// Determines whether a predicate exists asynchronously.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Returns <c>true</c> If the predicate returns successful; otherwise, <c>false</c>.</returns>
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Saves changes asynchronously.
        /// </summary>
        /// <exception cref="SavingChangesFailedException">If it fails to the save changes.</exception>
        /// <returns>Returns <c>true</c> If the changes are successful; otherwise, <c>false</c>.</returns>
        Task<bool> SaveChangesAsync();
    }
}
