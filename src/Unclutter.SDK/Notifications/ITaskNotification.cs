using System.Threading;
using Unclutter.SDK.Common;
using Unclutter.SDK.Images;
using Unclutter.SDK.Progress;

namespace Unclutter.SDK.Notifications
{
    public interface ITaskNotification : INotification
    {
        /// <summary>
        /// Get a controller object to set progress
        /// </summary>
        IProgressController Controller { get; }
    }

    public interface ITaskNotificationConfig
    {
        /// <summary>
        /// Create a new instance of <see cref="ITaskNotification"/>
        /// </summary>
        /// <returns></returns>
        ITaskNotification Create();

        /// <summary>
        /// Set the title text.
        /// </summary>
        ITaskNotificationConfig Title(string title);

        /// <summary>
        /// Set the message text
        /// </summary>
        ITaskNotificationConfig Message(string message);

        /// <summary>
        /// Display an icon
        /// </summary>
        ITaskNotificationConfig Icon(IconType iconType = IconType.Custom, ImageReference customIcon = null);

        /// <summary>
        /// Display a cancel button
        /// </summary>
        /// <param name="cancellationTokenSource">The CancellationTokenSource to set the <see cref="IProgressController.CancellationTokenSource"/></param>
        /// <param name="autoCancel">true to invoke Cancel on the associated <see cref="IProgressController.CancellationTokenSource"/>, false otherwise</param>
        ITaskNotificationConfig CancelButton(CancellationTokenSource cancellationTokenSource = null, bool autoCancel = true);

        /// <summary>
        /// Set the progress to indeterminate, default is true
        /// </summary>
        ITaskNotificationConfig IsIndeterminate(bool defaultValue = true);
    }
}