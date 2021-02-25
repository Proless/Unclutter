namespace Unclutter.SDK.IModels
{
    public interface IUserProfile
    {
        long Id { get; }
        string Name { get; }
        string DownloadsDirectory { get; }
        IGameDetails Game { get; }
        IUserDetails User { get; }
        bool IsValid();
    }
}
