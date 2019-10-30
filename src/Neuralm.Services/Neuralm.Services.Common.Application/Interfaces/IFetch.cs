namespace Neuralm.Services.Common.Application.Interfaces
{
    /// <summary>
    /// Represents the <see cref="IFetch{T}"/> interface.
    /// Used to delay the dependency injection.
    /// </summary>
    /// <typeparam name="T">The type parameter.</typeparam>
    public interface IFetch<out T>
    {
        /// <summary>
        /// Fetches <see cref="T"/>.
        /// </summary>
        /// <returns>Returns <see cref="T"/>.</returns>
        T Fetch();
    }
}
