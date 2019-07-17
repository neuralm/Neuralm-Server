namespace Neuralm.Application.Interfaces
{
    /// <summary>
    /// The interface for the Factory pattern.
    /// </summary>
    /// <typeparam name="TResult">Factory output result.</typeparam>
    public interface IFactory<out TResult>
    {
        /// <summary>
        /// Creates a new instance of <see cref="TResult"/>.
        /// </summary>
        /// <returns>Returns a new instance of <see cref="TResult"/>.</returns>
        TResult Create();
    }

    /// <summary>
    /// The interface for the Factory pattern with an argument.
    /// </summary>
    /// <typeparam name="TResult">Factory output result.</typeparam>
    /// <typeparam name="TParameter">Factory argument.</typeparam>
    public interface IFactory<out TResult, in TParameter>
    {
        /// <summary>
        /// Creates a new instance of <see cref="TResult"/>.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>Returns a new instance of <see cref="TResult"/>.</returns>
        TResult Create(TParameter parameter);
    }

    /// <summary>
    /// The interface for the Factory pattern with two arguments.
    /// </summary>
    /// <typeparam name="TResult">Factory output result.</typeparam>
    /// <typeparam name="TParameter1">Factory argument 1.</typeparam>
    /// <typeparam name="TParameter2">Factory argument 2.</typeparam>
    public interface IFactory<out TResult, in TParameter1, in TParameter2>
    {
        /// <summary>
        /// Creates a new instance of <see cref="TResult"/>.
        /// </summary>
        /// <param name="parameter1">The parameter 1.</param>
        /// <param name="parameter2">The parameter 2.</param>
        /// <returns>Returns a new instance of <see cref="TResult"/>.</returns>
        TResult Create(TParameter1 parameter1, TParameter2 parameter2);
    }
}
