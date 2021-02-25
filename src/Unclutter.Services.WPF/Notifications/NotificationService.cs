using System;
using System.Collections.Generic;
using Unclutter.SDK.Common;
using Unclutter.SDK.IServices;
using Unclutter.SDK.Notifications;

namespace Unclutter.Services.WPF.Notifications
{
    public class NotificationService : INotificationService
    {
        /* Fields */
        private readonly List<Notification> _notifications = new List<Notification>();
        private readonly List<TaskNotification> _tasks = new List<TaskNotification>();

        private readonly Dictionary<BaseNotification, BaseNotificationConfig> _configs
            = new Dictionary<BaseNotification, BaseNotificationConfig>();

        /* Events */
        public event EventHandler<NotificationChanged> NotificationChanged;

        /* Properties */
        public IReadOnlyCollection<ITaskNotification> Tasks => _tasks;
        public IReadOnlyCollection<INotification> Notifications => _notifications;

        /* Methods */
        public INotificationConfig Information(string title, string message)
        {
            return InternalCreateNotificationConfig(NotificationType.Information, title, message);
        }
        public INotificationConfig Warning(string title, string message)
        {
            return InternalCreateNotificationConfig(NotificationType.Warning, title, message);
        }
        public INotificationConfig Error(string title, string message)
        {
            return InternalCreateNotificationConfig(NotificationType.Error, title, message);
        }
        public ITaskNotificationConfig Task(string title, string message)
        {
            var newTaskNotification = new TaskNotification
            {
                Type = NotificationType.Progress,
                Title = title,
                Text = message
            };

            var newTaskNotificationConfig = new TaskNotificationConfig(newTaskNotification);
            newTaskNotificationConfig.Created += OnNotificationCreated;

            _configs.Add(newTaskNotification, newTaskNotificationConfig);

            return newTaskNotificationConfig;
        }

        /* Helpers */
        private INotificationConfig InternalCreateNotificationConfig(NotificationType type, string title, string message)
        {
            var newNotification = new Notification
            {
                Type = type,
                Title = title,
                Text = message
            };

            SetIconId(newNotification);

            var newNotificationConfig = new NotificationConfig(newNotification);
            newNotificationConfig.Created += OnNotificationCreated;

            _configs.Add(newNotification, newNotificationConfig);

            return newNotificationConfig;
        }
        private void OnNotificationCreated(BaseNotification baseNotification)
        {
            if (baseNotification is null) return;

            baseNotification.Closed += OnNotificationClosed;

            switch (baseNotification)
            {
                case Notification notification:
                    _notifications.Add(notification);
                    break;
                case TaskNotification taskNotification:
                    _tasks.Add(taskNotification);
                    break;
            }

            NotificationChanged?.Invoke(this, new NotificationChanged(ChangeType.Add, baseNotification));
        }
        private void OnNotificationClosed(BaseNotification notification)
        {
            if (notification is null) return;

            notification.Closed -= OnNotificationClosed;

            // remove associated config
            if (_configs.TryGetValue(notification, out var config) && config != null)
            {
                config.Created -= OnNotificationCreated;
                _configs.Remove(notification);
            }

            NotificationChanged?.Invoke(this, new NotificationChanged(ChangeType.Remove, notification));
        }
        private void SetIconId(Notification notification)
        {
            notification.IconId = notification.Type switch
            {
                NotificationType.Information => "information",
                NotificationType.Warning => "alert",
                NotificationType.Error => "close-circle",
                _ => null
            };
        }
    }
}
