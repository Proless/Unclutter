using System;
using System.Threading;
using Unclutter.SDK.Notifications;

namespace Unclutter.Services.WPF.Notifications
{
    internal class TaskNotificationConfig : BaseNotificationConfig, ITaskNotificationConfig
    {
        /* Fields */
        private readonly TaskNotification _task;

        /* Event */
        public override event Action<BaseNotification> Created;

        /* Constructor */
        public TaskNotificationConfig(TaskNotification task)
        {
            _task = task;
        }

        /* Methods */
        public ITaskNotification Create()
        {
            Created?.Invoke(_task);
            return _task;
        }
        public ITaskNotificationConfig OnClicked(Action<ITaskNotification, NotificationAction> onClicked)
        {
            _task.OnClicked = (notification, action) => onClicked(notification as ITaskNotification, action);
            return this;
        }
        public ITaskNotificationConfig SupportsCancellation(CancellationTokenSource cancellationTokenSource = null)
        {
            _task.CancellationTokenSource = cancellationTokenSource;
            _task.IsCancelable = true;
            return this;
        }
        public ITaskNotificationConfig IsIndeterminate(bool defaultValue = true)
        {
            _task.IsIndeterminate = defaultValue;
            return this;
        }
        public ITaskNotificationConfig CanClose(bool defaultValue = true)
        {
            _task.CanClose = defaultValue;
            return this;
        }
    }
}
