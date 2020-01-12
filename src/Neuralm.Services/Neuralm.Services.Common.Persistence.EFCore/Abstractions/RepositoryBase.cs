using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Neuralm.Services.Common.Application.Interfaces;
using Neuralm.Services.Common.Domain;
using Neuralm.Services.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Neuralm.Services.Common.Persistence.EFCore.Abstractions
{
    /// <summary>
    /// Represents the <see cref="RepositoryBase{TEntity, TDbContext}"/> class.
    /// A base implementation of a Repository pattern using the <see cref="Microsoft.EntityFrameworkCore.DbContext"/> from EntityFramework.
    /// </summary>
    /// <typeparam name="TEntity">The entity the repository pattern is used with.</typeparam>
    /// <typeparam name="TDbContext">The DbContext the repository pattern is used with.</typeparam>
    public abstract class RepositoryBase<TEntity, TDbContext> : IRepository<TEntity> where TDbContext : DbContext where TEntity : class, IEntity
    {
        protected readonly TDbContext DbContext;
        protected readonly IEntityValidator<TEntity> EntityValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryBase{TEntity, TDbContext}"/> class.
        /// </summary>
        /// <param name="dbContext">The DbContext.</param>
        /// <param name="entityValidator">The entity validator.</param>
        protected RepositoryBase(TDbContext dbContext, IEntityValidator<TEntity> entityValidator)
        {
            DbContext = dbContext;
            EntityValidator = entityValidator;
        }

        /// <inheritdoc cref="IRepository{TEntity}.CreateAsync(TEntity)"/>
        public virtual async Task<(bool success, Guid id)> CreateAsync(TEntity entity)
        {
            bool saveSuccess = false;
            using EntityLoadLock.Releaser loadLock = EntityLoadLock.Shared.Lock();
            try
            {
                EntityValidator.Validate(entity);
                DbContext.Set<TEntity>().Add(entity);
                int saveResult = await DbContext.SaveChangesAsync();
                saveSuccess = Convert.ToBoolean(saveResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine(new CreatingEntityFailedException($"The entity of type {typeof(TEntity).Name} could not be created.", ex));
            }
            return (success: saveSuccess, id: entity.Id);
        }

        /// <inheritdoc cref="IRepository{TEntity}.GetPaginationAsync(int, int)"/>
        public virtual async Task<IEnumerable<TEntity>> GetPaginationAsync(int pageNumber, int pageSize)
        {
            using EntityLoadLock.Releaser loadLock = EntityLoadLock.Shared.Lock();
            return await DbContext.Set<TEntity>().Skip(pageNumber * pageSize).Take(pageSize).ToListAsync();
        }
        
        /// <inheritdoc cref="IRepository{TEntity}.CountAsync()"/>
        public virtual async Task<int> CountAsync()
        {
            return await DbContext.Set<TEntity>().CountAsync();
        }

        /// <inheritdoc cref="IRepository{TEntity}.DeleteAsync(TEntity)"/>
        public virtual async Task<bool> DeleteAsync(TEntity entity)
        {
            bool saveSuccess = false;
            using EntityLoadLock.Releaser loadLock = EntityLoadLock.Shared.Lock();
            try
            {
                if (!await DbContext.Set<TEntity>().ContainsAsync(entity))
                    throw new EntityNotFoundException($"The entity of type {typeof(TEntity).Name} could not be found.");

                DbContext.Set<TEntity>().Remove(entity);
                int saveResult = await DbContext.SaveChangesAsync();
                saveSuccess = Convert.ToBoolean(saveResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine(new DeletingEntityFailedException($"The entity of type {typeof(TEntity).Name} could not be deleted.", ex));
            }
            return saveSuccess;
        }

        /// <inheritdoc cref="IRepository{TEntity}.ExistsAsync(Expression{Func{TEntity, bool}})"/>
        public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            using EntityLoadLock.Releaser loadLock = EntityLoadLock.Shared.Lock();
            return await DbContext.Set<TEntity>().AnyAsync(predicate);
        }
        
        /// <inheritdoc cref="IRepository{TEntity}.ExistsAsync(Expression{Func{TEntity, bool}})"/>
        public async Task<bool> SaveChangesAsync()
        {
            using EntityLoadLock.Releaser loadLock = EntityLoadLock.Shared.Lock();
            bool saveSuccess = false;
            try
            {
                int saveResult = await DbContext.SaveChangesAsync();
                saveSuccess = Convert.ToBoolean(saveResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine(new SavingChangesFailedException("The changes failed to save.", ex));
            }
            return saveSuccess;
        }

        /// <inheritdoc cref="IRepository{TEntity}.FindManyAsync(Expression{Func{TEntity, bool}})"/>
        public virtual async Task<IEnumerable<TEntity>> FindManyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            using EntityLoadLock.Releaser loadLock = EntityLoadLock.Shared.Lock();
            return await DbContext.Set<TEntity>().Where(predicate).ToListAsync();
        }

        /// <inheritdoc cref="IRepository{TEntity}.FindSingleOrDefaultAsync(Expression{Func{TEntity, bool}})"/>
        public virtual async Task<TEntity> FindSingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            using EntityLoadLock.Releaser loadLock = EntityLoadLock.Shared.Lock();
            return await DbContext.Set<TEntity>().SingleOrDefaultAsync(predicate);
        }

        /// <inheritdoc cref="IRepository{TEntity}.GetAllAsync()"/>
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            using EntityLoadLock.Releaser loadLock = EntityLoadLock.Shared.Lock();
            return await DbContext.Set<TEntity>().ToListAsync();
        }

        /// <inheritdoc cref="IRepository{TEntity}.UpdateAsync(TEntity)"/>
        public virtual async Task<(bool success, Guid id, bool updated)> UpdateAsync(TEntity entity)
        {
            bool saveSuccess = false;
            using EntityLoadLock.Releaser loadLock = EntityLoadLock.Shared.Lock();
            EntityEntry<TEntity> entry = DbContext.Update(entity);
            try
            {
                int saveResult = await DbContext.SaveChangesAsync();
                saveSuccess = Convert.ToBoolean(saveResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine(new UpdatingEntityFailedException($"The entity of type {typeof(TEntity).Name} failed to update.", ex));
            }
            return (success: saveSuccess, id: entity.Id, updated: entry.State == EntityState.Modified);
        }
    }
}
