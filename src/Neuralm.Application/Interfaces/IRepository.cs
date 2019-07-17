using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Neuralm.Application.Interfaces
{
    /// <summary>
    /// The interface for a generic repository.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> FindSingleByExpressionAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> FindManyByExpressionAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<bool> DeleteAsync(TEntity entity);
        Task<bool> UpdateAsync(TEntity entity);
        Task<bool> CreateAsync(TEntity entity);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
