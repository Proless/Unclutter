namespace Unclutter.SDK.Services
{
    public interface IDirectoryService
    {
        string WorkingDirectory { get; }
        string DataDirectory { get; }
        string CacheDirectory { get; }
        string ExtensionsDirectory { get; }
        void EnsureDirectoryAccess(string dir);
    }
}