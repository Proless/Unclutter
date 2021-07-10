using System.Threading;
using Unclutter.SDK.Common;
using Unclutter.SDK.Images;
using Unclutter.SDK.Notifications;

namespace Unclutter.Services.Notifications.Task
{
    public class TaskNotificationConfig : BaseNotificationConfig, ITaskNotificationConfig
    {
        /* Fields */
        internal TaskNotification ConfiguredNotification { get; } = new TaskNotification();

        /* Methods */
        public ITaskNotification Create()
        {
            var notification = new TaskNotification
            {
                IconType = ConfiguredNotification.IconType,
                Message = ConfiguredNotification.Message,
                Icon = ConfiguredNotification.Icon,
                Title = ConfiguredNotification.Title,
                ActionLabel = ConfiguredNotification.ActionLabel,
                AutoCancel = ConfiguredNotification.AutoCancel,
                IsCancelable = ConfiguredNotification.IsCancelable,
                IsIndeterminate = ConfiguredNotification.IsIndeterminate,
                ProgressValue = ConfiguredNotification.ProgressValue,
                Controller = { CancellationTokenSource = ConfiguredNotification.Controller.CancellationTokenSource }
            };

            Created?.Invoke(notification);

            return notification;
        }
        public ITaskNotificationConfig Title(string title)
        {
            ConfiguredNotification.Title = title;
            return this;
        }
        public ITaskNotificationConfig Message(string message)
        {
            ConfiguredNotification.Message = message;
            return this;
        }
        public ITaskNotificationConfig Icon(IconType iconType = IconType.Custom, ImageReference customIcon = null)
        {
            ConfiguredNotification.IconType = iconType;
            ConfiguredNotification.Icon = customIcon;
            return this;
        }
        public ITaskNotificationConfig CancelButton(CancellationTokenSource cancellationTokenSource = null, bool autoCancel = true)
        {
            ConfiguredNotification.AutoCancel = autoCancel;
            ConfiguredNotification.IsCancelable = true;
            ConfiguredNotification.Controller.CancellationTokenSource = cancellationTokenSource;
            return this;
        }
        public ITaskNotificationConfig IsIndeterminate(bool defaultValue = true)
        {
            ConfiguredNotification.IsIndeterminate = defaultValue;
            return this;
        }
    }
}
