using Microsoft.Extensions.Configuration;
using Neuralm.Services.Common.Application;
using Neuralm.Services.Common.Mapping;
using Neuralm.Services.MessageQueue.Application.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Neuralm.Services.MessageQueue.NeuralmMQ
{
    /// <summary>
    /// Represents the <see cref="Program"/> class.
    /// </summary>
    public class Program
    {
        private static readonly CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();
        private IGenericServiceProvider _genericServiceProvider;
        private static int _canReadConsole;
        private static int _canExit;

        /// <summary>
        /// Initializes the <see cref="Program"/> class.
        /// </summary>
        /// <param name="args">The args.</param>
        public static async Task Main(string[] args)
        {
            Task task = new Program().RunAsync(CancellationTokenSource.Token);
            _ = Task.Run(() => task);

//            while (!CancellationTokenSource.IsCancellationRequested)
//            {
//                if (Interlocked.CompareExchange(ref _canReadConsole, 0, 0) == 0)
//                {
//                    await Task.Delay(500);
//                    continue;
//                }
//
//                if (Console.ReadKey().Key == ConsoleKey.Q)
//                {
//                    CancellationTokenSource.Cancel();
//                    continue;
//                }
//                Console.WriteLine("\nPress Q to shut down the message queue.");
//            }
            if (Interlocked.CompareExchange(ref _canExit, 1, 1) == 0)
            {
                try
                {
                    await task;
                }
                catch (Exception e)
                {
                    Console.WriteLine("RunAsync cancelled.");
                    Console.WriteLine(e);
                }
            }

            // Temporary fix, docker won't allow input...
            await Task.Delay(-1);
            Console.WriteLine("\nMessage queue has shut down");
            Console.WriteLine("Press any key to continue..");
            Console.ReadKey();
        }

        /// <summary>
        /// Runs the program asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        private async Task RunAsync(CancellationToken cancellationToken)
        {
            IConfiguration configuration;
            try
            {
                configuration = ConfigurationLoader.GetConfiguration("appsettings");
            }
            catch (Exception)
            {
                Console.WriteLine("Please check if you have a valid appsettings.json!");
                CancellationTokenSource.Cancel();
                return;
            }

            Startup startup = new Startup(CancellationTokenSource, 60);
            Console.WriteLine("Initializing...");
            await startup.InitializeAsync(configuration, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            Console.WriteLine("Finished initializing!");

            _genericServiceProvider = startup.GetGenericServiceProvider();

            IRegistryService registryService = _genericServiceProvider.GetService<IRegistryService>();
            _ = Task.Run(async () => await registryService.StartReceivingServiceEndPointsAsync(cancellationToken), cancellationToken);
            Console.WriteLine("Started RegistryService EndPoint.");

            IClientMessageProcessor clientMessageProcessor = _genericServiceProvider.GetService<IClientMessageProcessor>();
            _ = Task.Run(async () => await clientMessageProcessor.StartAsync(cancellationToken), cancellationToken);
            Console.WriteLine("Started client messaging EndPoint.");

            Interlocked.Exchange(ref _canReadConsole, 1);
            Interlocked.Exchange(ref _canExit, 1);
        }
    }
}
