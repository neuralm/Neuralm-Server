namespace Neuralm.Application.Interfaces
{
    /// <summary>
    /// The interface for validating Entities.
    /// </summary>
    /// <typeparam name="T">The Entity to validate.</typeparam>
    public interface IEntityValidator<in T> where T : class
    {
        bool Validate(T entity);
    }
}
