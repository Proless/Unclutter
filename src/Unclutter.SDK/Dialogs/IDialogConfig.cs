using System.Threading;

namespace Unclutter.SDK.Dialogs
{
    public interface IMessageDialogConfig
    {
        IMessageDialog Create();
        /// <summary>
        /// Display an optional check-box on the dialog
        /// </summary>
        /// <param name="label">The check-box label</param>
        /// <param name="isChecked">The check-box checked status, default is false</param>
        IMessageDialogConfig Option(string label, bool isChecked = false);

        /// <summary>
        /// Set the default close behavior, default is true
        /// </summary>
        IMessageDialogConfig CanClose(bool defaultValue = true);

        /// <summary>
        /// Display an icon
        /// </summary>
        IMessageDialogConfig Icon(DialogIcon iconType = DialogIcon.Custom, object customIcon = null);

        /// <summary>
        /// Display the left button
        /// </summary>
        /// <param name="label">The button label</param>
        IMessageDialogConfig LeftButton(string label);

        /// <summary>
        /// Display the middle button
        /// </summary>
        /// <param name="label">The button label</param>
        IMessageDialogConfig MidButton(string label);

        /// <summary>
        /// Display the right button
        /// </summary>
        /// <param name="label">The button label</param>
        IMessageDialogConfig RightButton(string label);
    }

    public interface ITaskDialogConfig
    {
        ITaskDialog Create();
        /// <summary>
        /// Display an optional check-box on the dialog
        /// </summary>
        /// <param name="label">The check-box label</param>
        /// <param name="isChecked">The check-box checked status, default is false</param>
        ITaskDialogConfig Option(string label, bool isChecked = false);

        /// <summary>
        /// Set the default close behavior, default is true
        /// </summary>
        ITaskDialogConfig CanClose(bool defaultValue = true);

        /// <summary>
        /// Display an icon
        /// </summary>
        ITaskDialogConfig Icon(DialogIcon iconType = DialogIcon.Custom, object customIcon = null);

        /// <summary>
        /// Display a cancel button, default is false
        /// </summary>
        /// <param name="label">The button label</param>
        /// <param name="cancellationTokenSource">The cancellation token used to cancel a running operation</param>
        ITaskDialogConfig CancelButton(string label, CancellationTokenSource cancellationTokenSource);

        /// <summary>
        /// Set the progress to indeterminate, default is true
        /// </summary>
        ITaskDialogConfig IsIndeterminate(bool defaultValue = true);
    }
}
