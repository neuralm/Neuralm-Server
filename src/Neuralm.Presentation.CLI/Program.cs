using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Neuralm.Application.Configurations;
using Neuralm.Application.Interfaces;
using Neuralm.Application.Messages.Dtos;
using Neuralm.Application.Messages.Requests;
using Neuralm.Application.Messages.Responses;
using Neuralm.Infrastructure.Interfaces;
using Neuralm.Infrastructure.MessageSerializers;
using Neuralm.Infrastructure.Networking;
using Neuralm.Mapping;
using Neuralm.Utilities;

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
            Startup startup = new Startup(CancellationTokenSource);
            Console.WriteLine("Initializing...");
            await startup.InitializeAsync(configuration, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            Console.WriteLine("Finished initializing!\n");

            Interlocked.Exchange(ref _canReadConsole, 1);
            Interlocked.Exchange(ref _canExit, 1);

            _genericServiceProvider = startup.GetGenericServiceProvider();
            GetEnabledTrainingRoomsResponse getEnabledTrainingRoomsResponse = await _genericServiceProvider.GetService<ITrainingRoomService>()
                .GetEnabledTrainingRoomsAsync(new GetEnabledTrainingRoomsRequest());
            foreach (TrainingRoomDto trainingRoomDto in getEnabledTrainingRoomsResponse.TrainingRooms)
                Console.WriteLine($"TrainingRoom:\n\tId: {trainingRoomDto.Id}\n\tName: {trainingRoomDto.Name}\n\tOwner: {trainingRoomDto.Owner.Username}");

            ServerConfiguration serverConfiguration = _genericServiceProvider.GetService<IOptions<ServerConfiguration>>().Value;
            TcpListener tcpListener = new TcpListener(IPAddress.Any, serverConfiguration.Port);
            tcpListener.Start();
            Console.WriteLine($"Started listening for clients on port: {serverConfiguration.Port}.");
            while (!cancellationToken.IsCancellationRequested)
            {
                TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();
                Console.WriteLine($"Accepted a new connection: \n\tLocalEndPoint: {tcpClient.Client.LocalEndPoint}\n\tRemoteEndPoint: {tcpClient.Client.RemoteEndPoint}");
                _ = Task.Run(async () =>
                {
                    IMessageProcessor messageProcessor = new ServerMessageProcessor(_genericServiceProvider.GetService<MessageToServiceMapper>());
                    IMessageSerializer messageSerializer = new JsonMessageSerializer();
                    SslTcpNetworkConnector networkConnector = new SslTcpNetworkConnector(messageSerializer, messageProcessor, tcpClient);
                    await networkConnector.AuthenticateAsServer(serverConfiguration.Certificate, CancellationToken.None);
                    networkConnector.Start();
                }, cancellationToken);
            }
        }
    }
}
