using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Unclutter.SDK.Notifications;
using Unclutter.Services.Notifications.Message;
using Unclutter.Services.Notifications.Task;

namespace Unclutter.Services.Notifications
{
    public class NotificationProvider : INotificationProvider
    {
        /* Fields */
        private readonly object _lock = new object();
        private readonly ObservableCollection<INotification> _notifications = new ObservableCollection<INotification>();
        private readonly ObservableCollection<ITaskNotification> _tasks = new ObservableCollection<ITaskNotification>();

        /* Properties */
        public ReadOnlyObservableCollection<INotification> Notifications { get; }
        public ReadOnlyObservableCollection<ITaskNotification> Tasks { get; }

        /* Events */
        public event Action NotificationsChanged;
        public event Action TasksChanged;

        /* Constructor */
        public NotificationProvider()
        {
            _notifications.CollectionChanged += (_, _) => NotificationsChanged?.Invoke();
            _tasks.CollectionChanged += (_, _) => TasksChanged?.Invoke();

            Notifications = new ReadOnlyObservableCollection<INotification>(_notifications);
            Tasks = new ReadOnlyObservableCollection<ITaskNotification>(_tasks);
        }

        /* Methods */
        public INotificationConfig Message()
        {
            return new NotificationConfig { Created = OnCreated };
        }
        public ITaskNotificationConfig Task()
        {
            return new TaskNotificationConfig { Created = OnCreated };
        }
        public void CloseAll(NotificationType type)
        {
            var toCloseList = new List<INotification>();

            lock (_lock)
            {
                toCloseList.AddRange(type == NotificationType.Message ? _notifications.ToArray() : _tasks.ToArray());
            }

            foreach (var notification in toCloseList)
            {
                notification.Close();
            }
        }
        public void CloseAll()
        {
            var toCloseList = new List<INotification>();

            lock (_lock)
            {
                toCloseList.AddRange(_notifications.ToArray());
                toCloseList.AddRange(_tasks.ToArray());
            }

            foreach (var notification in toCloseList)
            {
                notification.Close();
            }
        }

        /* Helpers */
        private void OnCreated(BaseNotification notification)
        {
            if (notification is null) return;

            notification.CloseRequested = OnCloseRequested;
            notification.ShowRequested = OnShowRequested;
        }
        private void OnShowRequested(BaseNotification notification)
        {
            switch (notification.Type)
            {
                case NotificationType.Message:
                case NotificationType.Error:
                case NotificationType.Warning:
                    lock (_lock)
                    {
                        _notifications.Add(notification);
                    }
                    break;
                case NotificationType.Task:
                    if (notification is ITaskNotification task)
                    {
                        lock (_lock)
                        {
                            _tasks.Add(task);
                        }
                    }
                    break;
            }
            notification.OnShowed();
        }
        private void OnCloseRequested(BaseNotification notification)
        {
            switch (notification.Type)
            {
                case NotificationType.Message:
                    lock (_lock)
                    {
                        _notifications.Remove(notification);
                    }
                    break;
                case NotificationType.Task:
                    if (notification is ITaskNotification task)
                    {
                        lock (_lock)
                        {
                            _tasks.Remove(task);
                        }
                    }
                    break;
            }
            notification.OnClosed();
        }
    }
}