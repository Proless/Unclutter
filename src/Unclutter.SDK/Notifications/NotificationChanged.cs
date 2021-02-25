using System;
using Unclutter.SDK.Common;

namespace Unclutter.SDK.Notifications
{
    public class NotificationChanged : EventArgs
    {
        public ChangeType ChangeType { get; }
        public INotification Notification { get; }

        public NotificationChanged(ChangeType changeType, INotification notification)
        {
            ChangeType = changeType;
            Notification = notification;
        }
    }
}
