namespace Neuralm.Services.UserService.Application.Interfaces
{
    /// <summary>
    /// Represents the <see cref="ISaltGenerator"/> interface.
    /// </summary>
    public interface ISaltGenerator
    {
        /// <summary>
        /// Generates a byte array to be used as salt.
        /// </summary>
        /// <returns>Returns a byte array to be used as salt.</returns>
        byte[] GenerateSalt();
    }
}
