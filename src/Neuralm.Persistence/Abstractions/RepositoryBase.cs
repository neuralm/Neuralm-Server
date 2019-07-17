using Neuralm.Application.Interfaces;
using Neuralm.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Neuralm.Persistence.Abstractions
{
    /// <summary>
    /// Represents the <see cref="RepositoryBase{TEntity,TDbContext}"/> class; a base implementation of a Repository pattern using the <see cref="Microsoft.EntityFrameworkCore.DbContext"/> from EntityFramework.
    /// </summary>
    /// <typeparam name="TEntity">The entity the repository pattern is used with.</typeparam>
    /// <typeparam name="TDbContext">The DbContext the repository pattern is used with.</typeparam>
    public abstract class RepositoryBase<TEntity, TDbContext> : IRepository<TEntity> where TDbContext : DbContext where TEntity : class
    {
        protected readonly TDbContext DbContext;
        protected readonly IEntityValidator<TEntity> EntityValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryBase{TEntity,TDbContext}"/> class.
        /// </summary>
        /// <param name="dbContext">The DbContext.</param>
        /// <param name="entityValidator">The entity validator.</param>
        protected RepositoryBase(TDbContext dbContext, IEntityValidator<TEntity> entityValidator)
        {
            DbContext = dbContext;
            EntityValidator = entityValidator;
        }

        /// <summary>
        /// Saves the provided Entity in the DbContext.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <exception cref="CreatingEntityFailedException">If it fails to the save changes to the DbContext.</exception>
        /// <returns><c>true</c> If the Entity is successfully saved in the DbContext; otherwise, <c>false</c>.</returns>
        public virtual async Task<bool> CreateAsync(TEntity entity)
        {
            bool saveSuccess;
            try
            {
                EntityValidator.Validate(entity);
                DbContext.Set<TEntity>().Add(entity);
                int saveResult = await DbContext.SaveChangesAsync();
                saveSuccess = Convert.ToBoolean(saveResult);
            }
            catch (Exception ex)
            {
                throw new CreatingEntityFailedException($"The entity of type {typeof(TEntity).Name} could not be created.", ex);
            }
            return saveSuccess;
        }

        /// <summary>
        /// Deletes the provided Entity from the DbContext.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <exception cref="EntityNotFoundException">If the provided Entity cannot be found.</exception>
        /// <exception cref="DeletingEntityFailedException">If it fails to the save changes to the DbContext.</exception>
        /// <returns><c>true</c> If the Entity is successfully deleted from the DbContext; otherwise, <c>false</c>.</returns>
        public virtual async Task<bool> DeleteAsync(TEntity entity)
        {
            if (!await DbContext.Set<TEntity>().ContainsAsync(entity))
                throw new EntityNotFoundException($"The entity of type {typeof(TEntity).Name} could not be found.");
            DbContext.Set<TEntity>().Remove(entity);
            bool saveSuccess;
            try
            {
                int saveResult = await DbContext.SaveChangesAsync();
                saveSuccess = Convert.ToBoolean(saveResult);
            }
            catch (Exception ex)
            {
                throw new DeletingEntityFailedException($"The entity of type {typeof(TEntity).Name} could not be deleted.", ex);
            }
            return saveSuccess;
        }

        /// <summary>
        /// Determines whether a predicate exists in the DbContext.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns><c>true</c> If the predicate returns successful from the DbContext; otherwise, <c>false</c>.</returns>
        public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbContext.Set<TEntity>().AnyAsync(predicate);
        }

        /// <summary>
        /// Finds many Entities in the DbContext using an expression.
        /// </summary>
        /// <param name="predicate">The expression.</param>
        /// <returns>The results of the expression.</returns>
        public virtual async Task<IEnumerable<TEntity>> FindManyByExpressionAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbContext.Set<TEntity>().Where(predicate).ToListAsync();
        }

        /// <summary>
        /// Finds a single Entity by an expression.
        /// </summary>
        /// <param name="predicate">The expression.</param>
        /// <exception cref="EntityNotFoundException">If the expression cannot find a single Entity.</exception>
        /// <returns>Returns the result of the expression.</returns>
        public virtual async Task<TEntity> FindSingleByExpressionAsync(Expression<Func<TEntity, bool>> predicate)
        {
            TEntity entity = await DbContext.Set<TEntity>().SingleOrDefaultAsync(predicate);
            if (entity == default)
                throw new EntityNotFoundException($"The entity of type {typeof(TEntity).Name} could not be found by the predicate.");
            return entity;
        }

        /// <summary>
        /// Gets all Entities in the DbContext.
        /// </summary>
        /// <returns>Returns all Entities in the DbContext.</returns>
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await DbContext.Set<TEntity>().ToListAsync();
        }

        /// <summary>
        /// Updates the provided Entity in the DbContext.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <exception cref="UpdatingEntityFailedException">If it fails to the save changes to the DbContext.</exception>
        /// <returns><c>true</c> If the Entity is successfully updated in the DbContext; otherwise, <c>false</c>.</returns>
        public virtual async Task<bool> UpdateAsync(TEntity entity)
        {
            DbContext.Update(entity);
            bool saveSuccess;
            try
            {
                int saveResult = await DbContext.SaveChangesAsync();
                saveSuccess = Convert.ToBoolean(saveResult);
            }
            catch (Exception ex)
            {
                throw new UpdatingEntityFailedException($"The entity of type {typeof(TEntity).Name} failed to update.", ex);
            }
            return saveSuccess;
        }
    }
}
