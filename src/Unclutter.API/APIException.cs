using System;
using System.Net;

namespace Unclutter.API
{
    public class APIException : Exception
    {
        public HttpStatusCode? StatusCode { get; }

        public APIException(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public APIException(HttpStatusCode? statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }

        public APIException(HttpStatusCode? statusCode, string message, Exception inner) : base(message, inner)
        {
            StatusCode = statusCode;
        }
    }
}
