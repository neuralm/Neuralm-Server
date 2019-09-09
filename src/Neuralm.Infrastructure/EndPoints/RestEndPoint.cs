using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Neuralm.Application.Configurations;
using Neuralm.Application.Interfaces;
using Neuralm.Application.Messages;
using Neuralm.Application.Messages.Requests;
using Neuralm.Infrastructure.Interfaces;

namespace Neuralm.Infrastructure.EndPoints
{
    /// <summary>
    /// Represents the <see cref="RestEndPoint"/> class.
    /// </summary>
    public class RestEndPoint : IRestEndPoint
    {
        private readonly HttpListener _httpListener;
        private readonly ServerConfiguration _serverConfiguration;
        private readonly ConcurrentDictionary<string, Type> _resourcesToTypesMap;

        /// <summary>
        /// Initializes an instance of the <see cref="RestEndPoint"/> class.
        /// </summary>
        /// <param name="serverConfiguration"></param>
        public RestEndPoint(IOptions<ServerConfiguration> serverConfiguration)
        {
            _serverConfiguration = serverConfiguration.Value;
            MessageTypeCache.LoadMessageTypeCache();
            _resourcesToTypesMap = new ConcurrentDictionary<string, Type>();
            _resourcesToTypesMap.TryAdd("/users/authenticate", typeof(AuthenticateRequest));
            _resourcesToTypesMap.TryAdd("/users/register", typeof(RegisterRequest));
            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add($"http://{_serverConfiguration.Host}:{_serverConfiguration.RestPort}/");
        }

        /// <inheritdoc cref="IRestEndPoint.StartAsync(CancellationToken, IRequestProcessor, IMessageSerializer)"/>
        public async Task StartAsync(CancellationToken cancellationToken, IRequestProcessor requestProcessor, IMessageSerializer messageSerializer)
        {
            _httpListener.Start();

            Console.WriteLine($"Started listening for Rest calls on port: {_serverConfiguration.RestPort}.");
            while (!cancellationToken.IsCancellationRequested)
            {
                HttpListenerContext context = await _httpListener.GetContextAsync();
                _ = Task.Run(async () =>
                {
                    HttpListenerRequest request = context.Request;

                    Console.WriteLine($"REST | Request on {request.RawUrl}");
                    Console.WriteLine($"Content Length: {(int)request.ContentLength64}");

                    HttpListenerResponse response = context.Response;
                    response.AddHeader("Access-Control-Allow-Origin", "*");
                    if (request.HttpMethod == "OPTIONS")
                    {
                        response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
                        response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept, X-Requested-With, Authorization");
                        response.AddHeader("Access-Control-Max-Age", "86400");
                        response.StatusCode = 200;
                        response.Close();
                        return;
                    }
                    
                    if (!_resourcesToTypesMap.TryGetValue(request.RawUrl, out Type type))
                    {
                        Console.WriteLine("Unknown request URL.");
                        byte[] bytes = Encoding.UTF8.GetBytes(@"{ ""Error"": ""Resource not found!"" }");
                        response.StatusCode = 404;
                        response.ContentLength64 = bytes.Length;
                        await response.OutputStream.WriteAsync(bytes, cancellationToken);
                        response.Close();
                        return;
                    }

                    if (!request.HasEntityBody)
                    {
                        Console.WriteLine("No client data was sent with the request.");
                        byte[] bytes = Encoding.UTF8.GetBytes(@"{ ""Error"": ""Empty body!"" }");
                        response.StatusCode = 400;
                        response.ContentLength64 = bytes.Length;
                        await response.OutputStream.WriteAsync(bytes, cancellationToken);
                        response.Close();
                        return;
                    }
                    
                    // Read request
                    int contentLength = (int) request.ContentLength64;  
                    byte[] memory = ArrayPool<byte>.Shared.Rent(contentLength);
                    await request.InputStream.ReadAsync(memory, cancellationToken);
                    IRequest requestBody = messageSerializer.Deserialize(memory.AsMemory(0, contentLength), type) as IRequest;

                    // Process request
                    IResponse responseBody = await requestProcessor.ProcessRequest(type, requestBody);

                    // Write response
                    Memory<byte> responseBytes = messageSerializer.Serialize(responseBody);
                    response.StatusCode = 200;
                    response.ContentLength64 = responseBytes.Length;
                    await response.OutputStream.WriteAsync(responseBytes, cancellationToken);
                    response.Close();
                    ArrayPool<byte>.Shared.Return(memory);
                }, cancellationToken);
            }
        }

        /// <inheritdoc cref="IRestEndPoint.StopAsync()"/>
        public Task StopAsync()
        {
            _httpListener.Stop();
            return Task.CompletedTask;
        }
    }
}
