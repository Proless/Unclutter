using Unclutter.SDK.Common;
using Unclutter.SDK.Images;
using Unclutter.SDK.Notifications;

namespace Unclutter.Services.Notifications.Message
{
    public class NotificationConfig : BaseNotificationConfig, INotificationConfig
    {
        /* Fields */
        public MessageNotification ConfiguredMessageNotification { get; } = new MessageNotification();

        /* Methods */
        public INotification Create()
        {
            var notification = new MessageNotification
            {
                IconType = ConfiguredMessageNotification.IconType,
                Message = ConfiguredMessageNotification.Message,
                Icon = ConfiguredMessageNotification.Icon,
                Title = ConfiguredMessageNotification.Title,
                ActionLabel = ConfiguredMessageNotification.ActionLabel
            };

            Created?.Invoke(notification);

            return notification;
        }
        public INotificationConfig Title(string title)
        {
            ConfiguredMessageNotification.Title = title;
            return this;
        }
        public INotificationConfig Message(string message)
        {
            ConfiguredMessageNotification.Message = message;
            return this;
        }
        public INotificationConfig Icon(IconType iconType = IconType.Custom, ImageReference customIcon = null)
        {
            ConfiguredMessageNotification.IconType = iconType;
            ConfiguredMessageNotification.Icon = customIcon;
            return this;
        }
        public INotificationConfig ActionButton(string label)
        {
            ConfiguredMessageNotification.ActionLabel = label;
            return this;
        }
    }
}
