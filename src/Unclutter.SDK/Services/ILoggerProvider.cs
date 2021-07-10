
using Unclutter.SDK.Common;

namespace Unclutter.SDK.Services
{
    public interface ILoggerProvider
    {
        /// <summary>
        /// Get the global application logger instance.
        /// </summary>
        ILogger GetInstance();

        /// <summary>
        /// Get a new or existing logger with the specified name
        /// </summary>
        /// <param name="name">The name of the logger, it must be a valid file name</param>
        /// <returns>The <see cref="ILogger"/> instance associated with the specified name, or <see langword="null" /> if the name is an invalid file name</returns>
        ILogger GetLogger(string name);
    }
}
