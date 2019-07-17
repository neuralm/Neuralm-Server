using System;

namespace Neuralm.Presentation.CLI
{
    /// <summary>
    /// Represents the <see cref="InitializationException"/> class.
    /// </summary>
    internal class InitializationException : Exception
    {
        public InitializationException(string message) : base(message)
        {

        }
    }
}
