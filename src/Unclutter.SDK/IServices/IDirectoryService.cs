namespace Unclutter.SDK.IServices
{
    public interface IDirectoryService
    {
        string LocalsDirectory { get; }
        string WorkingDirectory { get; }
        string DataDirectory { get; }
        string ExtensionsDirectory { get; }
        void InitializeDirectories();
        void EnsureDirectoryAccess(string dir);
    }
}