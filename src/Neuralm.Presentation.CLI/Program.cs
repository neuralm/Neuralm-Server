using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
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

            ServerConfiguration serverConfiguration = _genericServiceProvider.GetService<IOptions<ServerConfiguration>>().Value;

            List<Task> tasks = new List<Task>();
            Console.WriteLine("Would you like to start the ClientEndPoint? y/n");
            ConsoleKeyInfo keyInfo = await WaitForReadKey(cancellationToken);
            bool runClientEndPoint = keyInfo.KeyChar == 'y';
            if (runClientEndPoint)
                tasks.Add(Task.Run(() => RunClientEndPoint(cancellationToken, serverConfiguration), cancellationToken));
            Console.WriteLine();

            Console.WriteLine("Would you like to start the RestEndPoint? y/n");
            keyInfo = await WaitForReadKey(cancellationToken);
            bool runRestEndPoint = keyInfo.KeyChar == 'y';
            if (runRestEndPoint)
                tasks.Add(Task.Run(() => RunRestEndPoint(cancellationToken, serverConfiguration), cancellationToken));
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
        /// <param name="serverConfiguration">The server configuration.</param>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        private async Task RunRestEndPoint(CancellationToken cancellationToken, ServerConfiguration serverConfiguration)
        {
            HttpListener httpListener = new HttpListener();
            httpListener.Prefixes.Add($"http://{serverConfiguration.Host}:{serverConfiguration.RestPort}/");
            httpListener.Start();

            Console.WriteLine($"Started listening for Rest calls on port: {serverConfiguration.RestPort}.");
            while (!cancellationToken.IsCancellationRequested)
            {
                HttpListenerContext context = await httpListener.GetContextAsync();
                Console.WriteLine($"Accepted a new request: \n\tLocalEndPoint: {context.Request.LocalEndPoint}\n\tRemoteEndPoint: {context.Request.RemoteEndPoint}");
                _ = Task.Run(async () =>
                {
                    using StreamReader reader = new StreamReader(context.Request.InputStream);
                    HttpListenerRequest request = context.Request;
                    
                    string body = await reader.ReadToEndAsync();

                    Console.WriteLine(request.RawUrl);
                    Console.WriteLine(body);

                    HttpListenerResponse response = context.Response;

                    response.AddHeader("Access-Control-Allow-Origin", "*");
                    response.AddHeader("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
                    response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept, X-Requested-With, Authorization");
                    response.AddHeader("Access-Control-Max-Age", "86400");

                    response.StatusCode = 200;

                    byte[] bytes = Encoding.UTF8.GetBytes("true\n");
                    response.ContentLength64 = bytes.Length;
                    await response.OutputStream.WriteAsync(bytes, cancellationToken);
                }, cancellationToken);
            }
        }

        /// <summary>
        /// Runs the client endpoint.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="serverConfiguration">The server configuration.</param>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        private async Task RunClientEndPoint(CancellationToken cancellationToken, ServerConfiguration serverConfiguration)
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Any, serverConfiguration.ClientPort);
            tcpListener.Start();
            Console.WriteLine($"Started listening for clients on port: {serverConfiguration.ClientPort}.");
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
