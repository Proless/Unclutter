using System;
using System.Collections.Generic;
using Unclutter.SDK.Notifications;

namespace Unclutter.SDK.IServices
{
    public interface INotificationService
    {
        event EventHandler<NotificationChanged> NotificationChanged;
        IReadOnlyCollection<ITaskNotification> Tasks { get; }
        IReadOnlyCollection<INotification> Notifications { get; }
        INotificationConfig Information(string title, string message);
        INotificationConfig Warning(string title, string message);
        INotificationConfig Error(string title, string message);
        ITaskNotificationConfig Task(string title, string message);
    }
}
