using System;
using System.Net;

namespace Unclutter.NexusAPI.Exceptions
{
    /// <summary>
    /// Represents an API exception for all non success StatusCodes returned from the API server
    /// </summary>
    public class APIException : Exception
    {
        /// <summary>
        /// The HTTP status code returned form the API server
        /// </summary>
        public HttpStatusCode StatusCode { get; }
        public APIException(HttpStatusCode statusCode) { StatusCode = statusCode; }
        public APIException(string message, HttpStatusCode statusCode) : base(message) { StatusCode = statusCode; }
        public APIException(string message, HttpStatusCode statusCode, Exception inner) : base(message, inner) { StatusCode = statusCode; }
    }
}
