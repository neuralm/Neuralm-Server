using Neuralm.Services.Common.Application.Interfaces;
using Neuralm.Services.Common.Domain;
using Neuralm.Services.Common.Exceptions;
using Neuralm.Services.Common.Messages;
using Neuralm.Services.Common.Messages.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Neuralm.Services.MessageQueue.Infrastructure.Messaging
{
    /// <summary>
    /// Represents the <see cref="HttpNetworkConnector"/> class.
    /// </summary>
    public class HttpNetworkConnector : INetworkConnector, IDisposable
    {
        private readonly IMessageSerializer _messageSerializer;
        private readonly IMessageProcessor _messageProcessor;
        private readonly ILogger<HttpNetworkConnector> _logger;
        private readonly HttpClient _httpClient;
        
        /// <inheritdoc cref="INetworkConnector.EndPoint"/> 
        public EndPoint EndPoint { get; private set; }
        
        /// <inheritdoc cref="INetworkConnector.IsConnected"/> 
        public bool IsConnected { get; private set; }
        
        /// <inheritdoc cref="INetworkConnector.IsRunning"/> 
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpNetworkConnector"/> class.,
        /// </summary>
        /// <param name="messageSerializer">The message serializer.</param>
        /// <param name="messageProcessor">The message processor.</param>
        /// <param name="baseUrl">The base url for the network connector to send requests to.</param>
        /// <param name="accessTokenService">The access token service.</param>
        /// <param name="logger">The logger.</param>
        public HttpNetworkConnector(
            IMessageSerializer messageSerializer, 
            IMessageProcessor messageProcessor, 
            Uri baseUrl, 
            IAccessTokenService accessTokenService,
            ILogger<HttpNetworkConnector> logger)
        {
            _messageSerializer = messageSerializer ?? throw new ArgumentNullException(nameof(messageSerializer));
            _messageProcessor = messageProcessor ?? throw new ArgumentNullException(nameof(messageProcessor));
            _logger = logger;
            _httpClient = new HttpClient {BaseAddress = baseUrl};
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "MessageQueue"),
                new Claim(ClaimTypes.Role, "MessageQueue")
            };
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessTokenService.GenerateAccessToken(claims)}");
            EndPoint = new DnsEndPoint(baseUrl.Host, baseUrl.Port);
        }
        
        /// <inheritdoc cref="INetworkConnector.ConnectAsync(CancellationToken)"/> 
        public Task ConnectAsync(CancellationToken cancellationToken)
        {
            IsConnected = true;
            return Task.CompletedTask;
        }

        /// <inheritdoc cref="INetworkConnector.SendMessageAsync{TMessage}(TMessage, CancellationToken)"/> 
        public async Task SendMessageAsync<TMessage>(TMessage message, CancellationToken cancellationToken) where TMessage : IMessage
        {
            try
            {
                if (!IsRunning)
                    throw new NetworkConnectorIsNotYetStartedException("NetworkConnector has not started yet. Call Start() first");

                if (!(Attribute.GetCustomAttribute(message.GetType(), typeof(MessageAttribute)) is MessageAttribute messageAttribute))
                    throw new InvalidMessageException("Invalid message");

                HttpRequestMessage httpRequestMessage = null;
                string httpClientBaseAddress = _httpClient.BaseAddress + messageAttribute.Path;
                httpRequestMessage = messageAttribute.OriginalMethod switch
                {
                    "GetAll" => 
                        new HttpRequestMessage(messageAttribute.Method, httpClientBaseAddress),
                    "Get" when message is IGetRequest getRequest => 
                        new HttpRequestMessage(messageAttribute.Method, $"{httpClientBaseAddress}{getRequest.GetId}"),
                    "Get" when message is PaginationRequest paginationRequest => 
                        new HttpRequestMessage(messageAttribute.Method, $"{httpClientBaseAddress}{paginationRequest.PageNumber}/{paginationRequest.PageSize}"),
                    _ => 
                        new HttpRequestMessage(messageAttribute.Method, httpClientBaseAddress)
                    {
                        Content = new StringContent(_messageSerializer.SerializeToString(message), Encoding.UTF8, $"application/{_messageSerializer.SerializerType}")
                    }
                };

                httpRequestMessage.Headers.Add("Request-Id", message.Id.ToString());

                HttpResponseMessage response = await _httpClient.SendAsync(httpRequestMessage, cancellationToken);
                _logger.LogInformation($"REST RESPONSE: {await response.Content.ReadAsStringAsync()}");
                byte[] bytes = await response.Content.ReadAsByteArrayAsync();
                object msg = null;
                if (messageAttribute.OriginalMethod == "GetAll" || message is IPaginationRequest)
                {
                    msg = Activator.CreateInstance(messageAttribute.ResponseType);
                    PropertyInfo property = messageAttribute.ResponseType.GetProperties()
                        .Where(p => p.PropertyType.GenericTypeArguments.Length != 0)
                        .Where(p => p.PropertyType.GenericTypeArguments[0] != typeof(char))
                        .Single(info => info.PropertyType.GetInterfaces().Count(c => c == typeof(IEnumerable)) == 1);
                    object e = _messageSerializer.Deserialize(bytes, property.PropertyType);
                    property.SetValue(msg, e);
                }
                else
                {
                    msg = _messageSerializer.Deserialize(bytes, !response.IsSuccessStatusCode ? typeof(RestErrorResponse) : messageAttribute.ResponseType);
                }
                
                switch (msg)
                {
                    case IPaginationResponse paginationResponse:
                        paginationResponse.RequestId = message.Id;
                        paginationResponse.DateTime = DateTime.UtcNow;
                        paginationResponse.Success = true;
                        paginationResponse.PageNumber = GetIntegerHeaderValue("X-Paging-PageNumber");
                        paginationResponse.PageSize = GetIntegerHeaderValue("X-Paging-PageSize");
                        paginationResponse.PageCount = GetIntegerHeaderValue("X-Paging-PageCount");
                        paginationResponse.TotalRecords = GetIntegerHeaderValue("X-Paging-TotalRecordCount");
                        await _messageProcessor.ProcessMessageAsync(paginationResponse, this);
                        break;
                    case IResponseMessage responseMessage:
                    {
                        if (responseMessage.RequestId != message.Id)
                        {
                            responseMessage.RequestId = message.Id;
                            responseMessage.DateTime = DateTime.UtcNow;
                            responseMessage.Success = true;
                        }
                        await _messageProcessor.ProcessMessageAsync(responseMessage, this);
                        break;
                    }
                    default:
                    {
                        ErrorResponse errorResponse = new ErrorResponse
                        {
                            Id = Guid.NewGuid(),
                            RequestId = message.Id,
                            RequestName = message.GetType().FullName,
                            ResponseName = messageAttribute.ResponseType.FullName,
                            DateTime = DateTime.UtcNow,
                            Success = false,
                            Message = msg is RestErrorResponse restErrorResponse
                                ? restErrorResponse.title
                                : "Unknown error."
                        };
                        await _messageProcessor.ProcessMessageAsync(errorResponse, this);
                        _logger.LogError($"REST API error: {errorResponse}");
                        break;
                    }
                }
                
                int GetIntegerHeaderValue(string headerName) => int.Parse(response.Headers.First(header => header.Key == headerName).Value.First());
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, "The HttpNetworkConnector failed to send a message.");
            }
        }

        /// <inheritdoc cref="INetworkConnector.Start"/> 
        public void Start()
        {
            if (!IsConnected)
                throw new NetworkConnectorIsNotYetConnectedException("NetworkConnector has not connected yet. Call ConnectAsync() first.");
            IsRunning = true;
        }

        /// <inheritdoc cref="INetworkConnector.Stop"/> 
        public void Stop()
        {
            IsRunning = false;
        }
        
        /// <inheritdoc cref="IDisposable.Dispose"/> 
        public void Dispose()
        {
            _httpClient?.Dispose();
        }
        
        /// <summary>
        /// Represents the <see cref="RestErrorResponse"/> class.
        /// </summary>
        private class RestErrorResponse
        {
            /// <summary>
            /// Gets and sets the type.
            /// </summary>
            public string type { get; set; }
            
            /// <summary>
            /// Gets and sets the title.
            /// </summary>
            public string title { get; set; }
            
            /// <summary>
            /// Gets and sets the status.
            /// </summary>
            public int status { get; set; }
            
            /// <summary>
            /// Gets and sets the trace id,
            /// </summary>
            public string traceId { get; set; }
        }
    }
}