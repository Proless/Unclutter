using System.Net;

namespace Unclutter.NexusAPI.Exceptions
{
    /// <summary>
    /// This Exception is thrown whenever a Limit has been exceeded
    /// </summary>
	public class LimitsExceededException : APIException
    {
        /// <summary>
        /// The type of the limits that were exceeded.
        /// </summary>
        public LimitType LimitType { get; }
        public LimitsExceededException(HttpStatusCode statusCode, LimitType limitType) : base(statusCode)
        {
            LimitType = limitType;
        }
        public LimitsExceededException(string message, HttpStatusCode statusCode, LimitType limitType) : base(message, statusCode)
        {
            LimitType = limitType;
        }
    }
}
