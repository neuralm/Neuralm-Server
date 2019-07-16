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
    public class Program
    {
        private static readonly CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();
        private IGenericServiceProvider _genericServiceProvider;

        public static async Task Main(string[] args)
        {
            _ = new Program().RunAsync(CancellationTokenSource.Token);

            while (!CancellationTokenSource.IsCancellationRequested)
            {
                if (Console.ReadKey().Key == ConsoleKey.Q)
                    break;
                Console.WriteLine("\nPress Q to shut down the server.");
            }
            CancellationTokenSource.Cancel();
            Console.WriteLine("\nServer has shut down");
            Console.WriteLine("Press any key to continue..");
            Console.ReadKey();
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            IConfiguration configuration = null;
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

            Startup startup = new Startup();
            Console.WriteLine("Initializing...");
            await startup.InitializeAsync(configuration);
            Console.WriteLine("Finished initializing!\n");

            _genericServiceProvider = startup.GetServiceProvider();
            GetEnabledTrainingRoomsResponse getEnabledTrainingRoomsResponse = await _genericServiceProvider.GetService<ITrainingRoomService>()
                .GetEnabledTrainingRoomsAsync(new GetEnabledTrainingRoomsRequest());
            foreach (TrainingRoomDto trainingRoomDto in getEnabledTrainingRoomsResponse.TrainingRooms)
                Console.WriteLine($"TrainingRoom:\n\tId: {trainingRoomDto.Id}\n\tName: {trainingRoomDto.Name}\n\tOwner: {trainingRoomDto.Owner.Username}");
            ServerConfiguration serverConfiguration = _genericServiceProvider.GetService<IOptions<ServerConfiguration>>().Value;
            TcpListener tcpListener = new TcpListener(IPAddress.Loopback, serverConfiguration.Port);
            tcpListener.Start();
            Console.WriteLine($"Started listening for clients on port: {serverConfiguration.Port}.");
            while (!cancellationToken.IsCancellationRequested)
            {
                TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();
                Console.WriteLine($"Accepted a new connection: \n\tLocalEndPoint: {tcpClient.Client.LocalEndPoint}\n\tRemoteEndPoint: {tcpClient.Client.RemoteEndPoint}");
                _ = Task.Run(() =>
                {
                    IMessageProcessor messageProcessor = new ServerMessageProcessor(_genericServiceProvider.GetService<MessageToServiceMapper>());
                    IMessageSerializer messageSerializer = new JsonMessageSerializer();
                    INetworkConnector networkConnector = new TcpNetworkConnector(messageSerializer, messageProcessor, tcpClient);
                    networkConnector.Start();
                }, cancellationToken);
            }
        }
    }
}
