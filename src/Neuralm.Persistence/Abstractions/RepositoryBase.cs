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
    public abstract class RepositoryBase<TEntity, TDbContext> : IRepository<TEntity> where TDbContext : DbContext where TEntity : class
    {
        protected readonly TDbContext DbContext;
        protected readonly IEntityValidator<TEntity> EntityValidator;

        protected RepositoryBase(TDbContext dbContext, IEntityValidator<TEntity> entityValidator)
        {
            DbContext = dbContext;
            EntityValidator = entityValidator;
        }

        public virtual async Task<bool> CreateAsync(TEntity entity)
        {
            EntityValidator.Validate(entity);
            DbContext.Set<TEntity>().Add(entity);
            bool saveSuccess;
            try
            {
                int saveResult = await DbContext.SaveChangesAsync();
                saveSuccess = Convert.ToBoolean(saveResult);
            }
            catch (Exception ex)
            {
                throw new CreatingEntityFailedException($"The entity of type {typeof(TEntity).Name} could not be created.", ex);
            }
            return saveSuccess;
        }
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
        public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbContext.Set<TEntity>().AnyAsync(predicate);
        }
        public virtual async Task<IEnumerable<TEntity>> FindManyByExpressionAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbContext.Set<TEntity>().Where(predicate).ToListAsync();
        }
        public virtual async Task<TEntity> FindSingleByExpressionAsync(Expression<Func<TEntity, bool>> predicate)
        {
            TEntity entity = await DbContext.Set<TEntity>().SingleOrDefaultAsync(predicate);
            if (entity == default)
                throw new EntityNotFoundException($"The entity of type {typeof(TEntity).Name} could not be found by the predicate.");
            return entity;
        }
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await DbContext.Set<TEntity>().ToListAsync();
        }
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
