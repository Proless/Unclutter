#nullable enable
using Unclutter.SDK.Common;

namespace Unclutter.SDK.IServices
{
    public interface ILoggerProvider
    {
        ILogger GetAppLogger();
        ILogger? GetOrCreateLogger(string file);
    }
}
