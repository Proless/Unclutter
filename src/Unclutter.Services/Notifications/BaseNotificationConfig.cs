using System;

namespace Unclutter.Services.Notifications
{
    public class BaseNotificationConfig
    {
        internal Action<BaseNotification> Created { get; set; }
    }
}
