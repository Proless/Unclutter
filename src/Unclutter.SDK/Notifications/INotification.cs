using System;
using Unclutter.SDK.Common;
using Unclutter.SDK.Images;

namespace Unclutter.SDK.Notifications
{
    public interface INotification
    {
        /// <summary>
        /// The creation date and time of the <see cref="INotification"/>
        /// </summary>
        DateTimeOffset Created { get; }

        /// <summary>
        /// Determines the type of the notification
        /// </summary>
        NotificationType Type { get; }

        /// <summary>
        /// Show the notification
        /// </summary>
        /// <param name="onActionClicked">The action to execute when the action button is clicked,
        /// this is the cancel button when the <see cref="Type"/> is a Task</param>
        /// <param name="onClosed">The action to execute after the notification is closed</param>
        void Show(Action onActionClicked = null, Action onClosed = null);

        /// <summary>
        /// Close the notification
        /// </summary>
        void Close();
    }

    public interface INotificationConfig
    {
        /// <summary>
        /// Create a new instance of <see cref="INotification"/>
        /// </summary>
        /// <returns></returns>
        INotification Create();

        /// <summary>
        /// Set the title text.
        /// </summary>
        INotificationConfig Title(string title);

        /// <summary>
        /// Set the message text
        /// </summary>
        INotificationConfig Message(string message);

        /// <summary>
        /// Display an icon
        /// </summary>
        INotificationConfig Icon(IconType iconType = IconType.Custom, ImageReference customIcon = null);

        /// <summary>
        /// Display an action button
        /// </summary>
        /// <param name="label"></param>
        INotificationConfig ActionButton(string label);
    }
}
