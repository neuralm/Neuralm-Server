using System;
using System.Threading;
using System.Threading.Tasks;

namespace Neuralm.Utilities
{
    public static class ConsoleUtility
    {
        /// <summary>
        /// Waits for the <see cref="Console.ReadKey()"/> method to return on an available key.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable <see cref="Task"/> with type parameter <see cref="ConsoleKeyInfo"/>.</returns>
        public static async Task<ConsoleKeyInfo> WaitForReadKey(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (Console.KeyAvailable)
                {
                    return Console.ReadKey(false);
                }
                await Task.Delay(50, cancellationToken);
            }
            return new ConsoleKeyInfo((char)0, 0, false, false, false);
        }
    }
}
