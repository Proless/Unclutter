using System;

namespace Unclutter.SDK.Common
{
    /// <summary>
    /// A collection of information about an operation's progress
    /// </summary>
    public readonly struct ProgressReport
    {
        /// <summary>
        /// The progress status message
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// The progress completion percentage
        /// </summary>
        public double ProgressPercentage { get; }

        /// <summary>
        /// The progress status
        /// </summary>
        public OperationStatus Status { get; }

        /// <summary>
        /// The exception if any was thrown
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Determines if the the operation in progress threw an exception
        /// </summary>
        public bool HasError => Exception != null;

        /// <summary>
        /// Create a new instance of the <see cref="ProgressReport"/> struct
        /// </summary>
        /// <param name="message">The progress status message</param>
        /// <param name="progressPercentage">The progress completion percentage</param>
        /// <param name="status">The progress status</param>
        /// <param name="exception">The exception if any was thrown</param>
        public ProgressReport(string message, double progressPercentage = 0d, OperationStatus status = OperationStatus.Executing, Exception exception = null)
        {
            Message = message;
            ProgressPercentage = progressPercentage;
            Exception = exception;
            Status = status;
        }
    }
}
