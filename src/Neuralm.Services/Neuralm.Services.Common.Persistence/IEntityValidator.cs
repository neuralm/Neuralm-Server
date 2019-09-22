using Neuralm.Services.Common.Domain;

namespace Neuralm.Services.Common.Persistence
{
    /// <summary>
    /// Represents the <see cref="IEntityValidator{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The Entity to validate.</typeparam>
    public interface IEntityValidator<in T> where T : IEntity
    {
        /// <summary>
        /// Validates the provided Entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Returns <c>true</c> if the Entity is validate; otherwise, <c>false</c>.</returns>
        bool Validate(T entity);
    }
}
