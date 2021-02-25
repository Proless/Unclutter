using System;

namespace Unclutter.Services.WPF.Notifications
{
    internal abstract class BaseNotificationConfig
    {
        public abstract event Action<BaseNotification> Created;
    }
}
