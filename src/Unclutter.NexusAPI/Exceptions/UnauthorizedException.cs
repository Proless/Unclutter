using System;
using System.Net;

namespace Unclutter.NexusAPI.Exceptions
{
    public class UnauthorizedException : APIException
    {
        public UnauthorizedException(HttpStatusCode statusCode) : base(statusCode) { }
        public UnauthorizedException(string message, HttpStatusCode statusCode) : base(message, statusCode) { }
        public UnauthorizedException(string message, HttpStatusCode statusCode, Exception inner) : base(message, statusCode, inner) { }
    }
}
