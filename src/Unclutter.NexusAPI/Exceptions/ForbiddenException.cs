using System;
using System.Net;

namespace Unclutter.NexusAPI.Exceptions
{
    /// <summary>
    /// Exception 
    /// </summary>
	public class ForbiddenException : APIException
	{
        public ForbiddenException(HttpStatusCode statusCode) : base(statusCode) { }
        public ForbiddenException(string message, HttpStatusCode statusCode) : base(message, statusCode) { }
        public ForbiddenException(string message, HttpStatusCode statusCode, Exception inner) : base(message, statusCode, inner) { }
	}
}
