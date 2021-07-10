namespace Unclutter.SDK.Notifications
{
    public interface INotificationCenter
    {
        bool IsOpen { get; }
        void Open();
        void Open(NotificationType type);
        void Close();
    }
}