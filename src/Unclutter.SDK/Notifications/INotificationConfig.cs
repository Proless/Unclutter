using System;
using System.Threading;

namespace Unclutter.SDK.Notifications
{
    public interface INotificationConfig
    {
        /// <summary>
        /// Create and show the notification in the notification center
        /// </summary>
        INotification Create();
        /// <summary>
        /// Register a click handler action, which is invoked if a button on the notification is clicked. <br/>
        /// The first parameter is the <see cref="INotification"/> instance, the second parameter <see cref="NotificationAction"/> represents the clicked button
        /// </summary>
        /// <param name="onClicked">The action to execute if a button is clicked</param>
        INotificationConfig OnClicked(Action<INotification, NotificationAction> onClicked);
        /// <summary>
        /// Display an optional check-box on the notification
        /// </summary>
        /// <param name="optionText">The check-box text</param>
        /// <param name="isChecked">The check-box checked status, default is false</param>
        /// <returns></returns>
        INotificationConfig Option(string optionText, bool isChecked = false);
        /// <summary>
        /// Display a MaterialDesign Icon, you only need to pass the corresponding name from https://materialdesignicons.com/ <br/>
        /// Passing null removes the icon
        /// </summary>
        /// <param name="iconId">The name of the icon from  https://materialdesignicons.com/ </param>
        INotificationConfig Icon(string iconId);
        /// <summary>
        /// Display an extra button
        /// </summary>
        /// <param name="buttonText">The button text</param>
        INotificationConfig LeftButton(string buttonText);
        /// <summary>
        /// Display a button
        /// </summary>
        /// <param name="buttonText">The button text</param>
        INotificationConfig RightButton(string buttonText);
        /// <summary>
        /// Set the default close behavior, default is true
        /// </summary>
        INotificationConfig CanClose(bool defaultValue = true);
    }

    public interface ITaskNotificationConfig
    {
        /// <summary>
        /// Creates and shows the notification in the notification center
        /// </summary>
        ITaskNotification Create();
        /// <summary>
        /// Registers a click handler action, which is invoked if a button on the notification is clicked. <br/>
        /// The first parameter is the <see cref="ITaskNotification"/> instance, the second parameter <see cref="NotificationAction"/> represents the clicked button
        /// </summary>
        /// <param name="onClicked">The action to execute if a button is clicked</param>
        ITaskNotificationConfig OnClicked(Action<ITaskNotification, NotificationAction> onClicked);
        /// <summary>
        /// Display a cancel button, default is false
        /// </summary>
        /// <param name="cancellationTokenSource">The cancellation token to call its <see cref="CancellationTokenSource.Cancel()"/> method</param>
        ITaskNotificationConfig SupportsCancellation(CancellationTokenSource cancellationTokenSource = null);
        /// <summary>
        /// Set the progress to indeterminate, default is true
        /// </summary>
        ITaskNotificationConfig IsIndeterminate(bool defaultValue = true);
        /// <summary>
        /// Set the default close behavior, default is true
        /// </summary>
        ITaskNotificationConfig CanClose(bool defaultValue = true);
    }
}
