using System;
using System.Threading;
using System.Threading.Tasks;
using Unclutter.SDK.Common;
using Unclutter.SDK.Images;
using Unclutter.SDK.Progress;

namespace Unclutter.SDK.Dialogs
{
    public interface ITaskDialog
    {
        /// <summary>
        /// Get a controller object to report progress
        /// </summary>
        IProgressController Controller { get; }

        /// <summary>
        /// Determines if the option check-box is checked
        /// </summary>
        bool IsOptionChecked { get; }

        /// <summary>
        /// Determines if the dialog can be closed
        /// </summary>
        bool CanClose { get; set; }

        /// <summary>
        /// Display as a modal Dialog
        /// </summary>
        Task<IProgressController> ShowDialogAsync(Action<ITaskDialog, DialogAction> onClicked = null);

        /// <summary>
        /// Display as a modal Dialog in the specified DialogHost.
        /// </summary>
        Task<IProgressController> ShowDialogAsync(string hostId, Action<ITaskDialog, DialogAction> onClicked = null);

        /// <summary>
        /// Close the dialog regardless of the <see cref="CanClose"/> value
        /// </summary>
        void Close();
    }

    public interface ITaskDialogConfig
    {
        /// <summary>
        /// Create a new instance of <see cref="ITaskDialog"/>.
        /// </summary>
        ITaskDialog Create();

        /// <summary>
        /// Set the title text.
        /// </summary>
        ITaskDialogConfig Title(string title);

        /// <summary>
        /// Set the message text
        /// </summary>
        ITaskDialogConfig Message(string message);

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
        ITaskDialogConfig Icon(IconType iconType = IconType.Custom, ImageReference customIcon = null);

        /// <summary>
        /// Display a cancel button, default is false
        /// </summary>
        /// <param name="cancellationTokenSource">The CancellationTokenSource to set the <see cref="IProgressController.CancellationTokenSource"/></param>
        /// <param name="autoCancel">true to invoke Cancel on the associated <see cref="IProgressController.CancellationTokenSource"/>, false otherwise</param>
        ITaskDialogConfig CancelButton(CancellationTokenSource cancellationTokenSource = null, bool autoCancel = true);

        /// <summary>
        /// Set the progress to indeterminate, default is true
        /// </summary>
        ITaskDialogConfig IsIndeterminate(bool defaultValue = true);
    }
}
