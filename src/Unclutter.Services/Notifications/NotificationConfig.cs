using System;
using Unclutter.SDK.Notifications;

namespace Unclutter.Services.Notifications
{
    internal class NotificationConfig : BaseNotificationConfig, INotificationConfig
    {
        /* Fields */
        private readonly Notification _notification;

        /* Event */
        public override event Action<BaseNotification> Created;

        /* Constructor */
        public NotificationConfig(Notification notification)
        {
            _notification = notification;
        }

        /* Methods */
        public INotification Create()
        {
            Created?.Invoke(_notification);
            return _notification;
        }
        public INotificationConfig OnClicked(Action<INotification, NotificationAction> onClicked)
        {
            _notification.OnClicked = onClicked;
            return this;
        }
        public INotificationConfig Option(string optionText, bool isChecked = false)
        {
            _notification.IsOptionChecked = isChecked;
            _notification.OptionText = optionText;
            return this;
        }
        public INotificationConfig Icon(string iconId)
        {
            _notification.IconId = iconId;
            return this;
        }
        public INotificationConfig LeftButton(string buttonText)
        {
            _notification.LeftButtonText = buttonText;
            return this;
        }
        public INotificationConfig RightButton(string buttonText)
        {
            _notification.RightButtonText = buttonText;
            return this;
        }
        public INotificationConfig CanClose(bool defaultValue = true)
        {
            _notification.CanClose = defaultValue;
            return this;
        }

    }
}
