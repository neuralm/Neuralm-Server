namespace Neuralm.Application.Interfaces
{
    /// <summary>
    /// Represents the <see cref="IEntityValidator{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The Entity to validate.</typeparam>
    public interface IEntityValidator<in T> where T : class
    {
        /// <summary>
        /// Validates the provided Entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Returns <c>true</c> if the Entity is validate; otherwise, <c>false</c>.</returns>
        bool Validate(T entity);
    }
}
