using Microsoft.Extensions.Configuration;
using Neuralm.Application.Interfaces;
using Neuralm.Application.Messages.Dtos;
using Neuralm.Application.Messages.Requests;
using Neuralm.Application.Messages.Responses;
using Neuralm.Infrastructure.Interfaces;
using Neuralm.Mapping;
using Neuralm.Utilities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static Neuralm.Utilities.ConsoleUtility;

namespace Neuralm.Presentation.CLI
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
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        public static async Task Main(string[] args)
        {
            Task task = new Program().RunAsync(CancellationTokenSource.Token);
            _ = Task.Run(() => task);

            while (!CancellationTokenSource.IsCancellationRequested)
            {
                if (Interlocked.CompareExchange(ref _canReadConsole, 0, 0) == 0)
                {
                    await Task.Delay(500);
                    continue;
                }

                if (Console.ReadKey().Key == ConsoleKey.Q)
                {
                    CancellationTokenSource.Cancel();
                    continue;
                }
                Console.WriteLine("\nPress Q to shut down the server.");
            }
            if (Interlocked.CompareExchange(ref _canExit, 1, 1) == 0)
            {
                try
                {
                    await task;
                }
                catch (Exception)
                {
                    Console.WriteLine("RunAsync cancelled.");
                }
            }

            Console.WriteLine("\nServer has shut down");
            Console.WriteLine("Press any key to continue..");
            Console.ReadKey();
        }

        /// <summary>
        /// Runs the <see cref="Program"/> asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        private async Task RunAsync(CancellationToken cancellationToken)
        {
            IConfiguration configuration;
            try
            {
                configuration = ConfigurationLoader.GetConfiguration("appSettings");
            }
            catch (Exception)
            {
                Console.WriteLine("Please check if you have a valid appSettings.json!");
                CancellationTokenSource.Cancel();
                return;
            }
            Startup startup = new Startup(CancellationTokenSource, 60);
            Console.WriteLine("Initializing...");
            await startup.InitializeAsync(configuration, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            Console.WriteLine("Finished initializing!\n");

            _genericServiceProvider = startup.GetGenericServiceProvider();
            GetEnabledTrainingRoomsResponse getEnabledTrainingRoomsResponse = await _genericServiceProvider.GetService<ITrainingRoomService>()
                .GetEnabledTrainingRoomsAsync(new GetEnabledTrainingRoomsRequest());
            foreach (TrainingRoomDto trainingRoomDto in getEnabledTrainingRoomsResponse.TrainingRooms)
                Console.WriteLine($"TrainingRoom:\n\tId: {trainingRoomDto.Id}\n\tName: {trainingRoomDto.Name}\n\tOwner: {trainingRoomDto.Owner?.Username}");

            MessageToServiceMapper messageToServiceMapper = _genericServiceProvider.GetService<MessageToServiceMapper>();
            IMessageSerializer messageSerializer = _genericServiceProvider.GetService<IMessageSerializer>();
            IMessageProcessor messageProcessor = new ServerMessageProcessor(messageToServiceMapper);

            List<Task> tasks = new List<Task>();
            Console.WriteLine("Would you like to start the ClientEndPoint? y/n");
            ConsoleKeyInfo keyInfo = await WaitForReadKey(cancellationToken);
            bool runClientEndPoint = keyInfo.KeyChar == 'y';
            if (runClientEndPoint)
                tasks.Add(Task.Run(() => RunClientEndPoint(cancellationToken, messageProcessor, messageSerializer), cancellationToken));
            Console.WriteLine();

            Console.WriteLine("Would you like to start the RestEndPoint? y/n");
            keyInfo = await WaitForReadKey(cancellationToken);
            bool runRestEndPoint = keyInfo.KeyChar == 'y';
            if (runRestEndPoint)
                tasks.Add(Task.Run(() => RunRestEndPoint(cancellationToken, messageProcessor, messageSerializer), cancellationToken));
            Console.WriteLine();

            Interlocked.Exchange(ref _canReadConsole, 1);
            Interlocked.Exchange(ref _canExit, 1);

            await Task.WhenAll(tasks).ContinueWith(task =>
            {
                if (task.IsCanceled)
                    CancellationTokenSource.Cancel();
            }, cancellationToken);
        }

        /// <summary>
        /// Runs the REST endpoint.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="requestProcessor">The request processor.</param>
        /// <param name="messageSerializer">The message serializer.</param>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        private async Task RunRestEndPoint(CancellationToken cancellationToken, IRequestProcessor requestProcessor, IMessageSerializer messageSerializer)
        {
            IRestEndPoint restEndPoint = _genericServiceProvider.GetService<IRestEndPoint>();
            await restEndPoint.StartAsync(cancellationToken, requestProcessor, messageSerializer);
        }

        /// <summary>
        /// Runs the client endpoint.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="messageProcessor">The message processor.</param>
        /// <param name="messageSerializer">The message serializer.</param>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        private async Task RunClientEndPoint(CancellationToken cancellationToken, IMessageProcessor messageProcessor, IMessageSerializer messageSerializer)
        {
            IClientEndPoint clientEndPoint = _genericServiceProvider.GetService<IClientEndPoint>();
            await clientEndPoint.StartAsync(cancellationToken, messageProcessor, messageSerializer);
        }
    }
}
