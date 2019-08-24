using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;
using System.Threading.Tasks;

namespace Neuralm.Persistence.Extensions
{
    /// <summary>
    /// Represents the <see cref="DbContextExtensions"/> class.
    /// </summary>
    public static class DbContextExtensions
    {
        /// <summary>
        /// Refreshes the entity on the current database context.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <param name="context">The database context.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>Returns the latest version of the entity.</returns>
        public static async Task<TEntity> RefreshEntityAsync<TEntity>(this DbContext context, TEntity entity) where TEntity : class
        {
            EntityEntry<TEntity> entry = context.Entry(entity);

            // If entry is already detached there is no need to continue.
            if (entry.State == EntityState.Detached)
                return entity;

            // Prepare key values to find the entity in the context.
            object[] keyValues = entry.Metadata.FindPrimaryKey().Properties.Select(x => x.GetGetter().GetClrValue(entity)).ToArray();

            // Set the entry of the entity to Detached so that it gets removed from cache.
            entry.State = EntityState.Detached;

            // Get the entity from the database using the key values.
            TEntity newEntity = await context.Set<TEntity>().FindAsync(keyValues);
            EntityEntry<TEntity> newEntityEntry = context.Entry(newEntity);

            // Update each property in the original entity entry.
            foreach (IProperty prop in newEntityEntry.Metadata.GetProperties())
            {
                prop.GetSetter().SetClrValue(entity, prop.GetGetter().GetClrValue(newEntity));
            }

            // Detach the newly fetched entity entry.
            newEntityEntry.State = EntityState.Detached;

            // Update the entry to be unchanged.
            entry.State = EntityState.Unchanged;

            return entity;
        }
    }
}
