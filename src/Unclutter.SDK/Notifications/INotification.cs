using System;
using Unclutter.SDK.Common;

namespace Unclutter.SDK.Notifications
{
    public interface INotification
    {
        /// <summary>
        /// Determines whether a notification should be closed after <see cref="OnClicked"/> action is executed, default is true
        /// </summary>
        bool CanClose { get; set; }

        /// <summary>
        /// Determines whether the check-box was checked when <see cref="OnClicked"/> got invoked
        /// </summary>
        bool IsOptionChecked { get; }

        /// <summary>
        /// The action to execute when a button is clicked on the Notification
        /// </summary>
        Action<INotification, NotificationAction> OnClicked { get; set; }

        /// <summary>
        /// The type of the Notification, which can be used for filtering
        /// </summary>
        NotificationType Type { get; }

        /// <summary>
        /// Close and remove from the notification center, note that this doesn't invoke the <see cref="OnClicked"/> action
        /// </summary>
        public void Close();
    }

    public interface ITaskNotification : INotification, IProgressController { }
}
