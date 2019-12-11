using System;
using System.Net.Http;

namespace Neuralm.Services.Common.Messages
{
    /// <summary>
    /// Represents the <see cref="MessageAttribute"/> class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class MessageAttribute : Attribute
    {
        /// <summary>
        /// Gets the http method.
        /// </summary>
        public HttpMethod Method { get; }
        
        /// <summary>
        /// Gets the original method.
        /// </summary>
        public string OriginalMethod { get; }
        
        /// <summary>
        /// Gets the path.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Gets the response type.
        /// </summary>
        public Type ResponseType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageAttribute"/> class.
        /// </summary>
        /// <param name="method">The http method.</param>
        /// <param name="path">The path.</param>
        /// <param name="responseType">The response type.</param>
        public MessageAttribute(string method, string path, Type responseType)
        {
            OriginalMethod = method;
            Method = new HttpMethod(method.Replace("All", ""));
            Path = path;
            ResponseType = responseType;
        }
    }
}