namespace Neuralm.Application.Interfaces
{
    /// <summary>
    /// The interface for a salt generator.
    /// </summary>
    public interface ISaltGenerator
    {
        byte[] GenerateSalt();
    }
}
