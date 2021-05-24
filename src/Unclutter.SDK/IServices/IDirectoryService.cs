using Unclutter.SDK.Loader;

namespace Unclutter.SDK.IServices
{
    public interface IDirectoryService : ILoader
    {
        string WorkingDirectory { get; }
        string DataDirectory { get; }
        string ExtensionsDirectory { get; }
        void EnsureDirectoryAccess(string dir);
    }
}