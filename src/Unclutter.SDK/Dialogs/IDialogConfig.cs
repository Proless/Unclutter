using System;
using System.Threading;

namespace Unclutter.SDK.Dialogs
{
    public interface IDialogConfig
    {
        /// <summary>
        /// Build the dialog.
        /// </summary>
        IDialog Build();

        /// <summary>
        /// Register a click handler action, which is invoked if a button on the dialog is clicked. <br/>
        /// The first parameter is the <see cref="IDialog"/> instance, the second parameter <see cref="DialogAction"/> represents the clicked button
        /// </summary>
        /// <param name="onClicked">The action to execute if a button is clicked</param>
        IDialogConfig OnClicked(Action<IDialog, DialogAction> onClicked);

        /// <summary>
        /// Display an optional check-box on the dialog
        /// </summary>
        /// <param name="label">The check-box label</param>
        /// <param name="isChecked">The check-box checked status, default is false</param>
        /// <returns></returns>
        IDialogConfig Option(string label, bool isChecked = false);

        /// <summary>
        /// Display the left button
        /// </summary>
        /// <param name="label">The button label</param>
        IDialogConfig LeftButton(string label);

        /// <summary>
        /// Display the middle button
        /// </summary>
        /// <param name="label">The button label</param>
        IDialogConfig MidButton(string label);

        /// <summary>
        /// Display the right button
        /// </summary>
        /// <param name="label">The button label</param>
        IDialogConfig RightButton(string label);

        /// <summary>
        /// Set the default close behavior, default is true
        /// </summary>
        IDialogConfig CanClose(bool defaultValue = true);
    }

    public interface ITaskDialogConfig
    {
        /// <summary>
        /// Build the dialog
        /// </summary>
        ITaskDialog Build();

        /// <summary>
        /// Display an optional check-box on the dialog
        /// </summary>
        /// <param name="label">The check-box label</param>
        /// <param name="isChecked">The check-box checked status, default is false</param>
        /// <returns></returns>
        ITaskDialogConfig Option(string label, bool isChecked = false);

        /// <summary>
        /// Registers a click handler action, which is invoked if a button on the dialog is clicked. <br/>
        /// The first parameter is the <see cref="ITaskDialog"/> instance, the second parameter <see cref="DialogAction"/> represents the clicked button
        /// </summary>
        /// <param name="onClicked">The action to execute if a button is clicked</param>
        ITaskDialogConfig OnClicked(Action<ITaskDialog, DialogAction> onClicked);

        /// <summary>
        /// Display a cancel button, default is false
        /// </summary>
        /// <param name="label">The button label</param>
        /// <param name="cancellationTokenSource">The cancellation token to call its <see cref="CancellationTokenSource.Cancel()"/> method</param>
        ITaskDialogConfig CancelButton(string label, CancellationTokenSource cancellationTokenSource = null);

        /// <summary>
        /// Set the progress to indeterminate, default is true
        /// </summary>
        ITaskDialogConfig IsIndeterminate(bool defaultValue = true);

        /// <summary>
        /// Set the default close behavior, default is true
        /// </summary>
        ITaskDialogConfig CanClose(bool defaultValue = true);
    }
}
