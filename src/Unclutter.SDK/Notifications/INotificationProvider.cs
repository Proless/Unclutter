using System;
using System.Collections.ObjectModel;

namespace Unclutter.SDK.Notifications
{
    public interface INotificationProvider
    {
        ReadOnlyObservableCollection<INotification> Notifications { get; }
        ReadOnlyObservableCollection<ITaskNotification> Tasks { get; }
        event Action NotificationsChanged;
        event Action TasksChanged;
        INotificationConfig Message();
        ITaskNotificationConfig Task();
        void CloseAll(NotificationType type);
        void CloseAll();
    }
}
