using System;

namespace Unclutter.Services.Notifications
{
    internal abstract class BaseNotificationConfig
    {
        public abstract event Action<BaseNotification> Created;
    }
}
