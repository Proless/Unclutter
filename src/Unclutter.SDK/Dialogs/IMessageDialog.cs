using System;
using System.Threading.Tasks;
using Unclutter.SDK.Common;
using Unclutter.SDK.Images;

namespace Unclutter.SDK.Dialogs
{
    public interface IMessageDialog
    {
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
        Task ShowDialogAsync(Action<IMessageDialog, DialogAction> onClicked = null);

        /// <summary>
        /// Display as a modal Dialog in the specified DialogHost.
        /// </summary>
        Task ShowDialogAsync(string hostId, Action<IMessageDialog, DialogAction> onClicked = null);

        /// <summary>
        /// Display as modal Dialog and return immediately 
        /// </summary>
        Task ShowAsync(Action<IMessageDialog, DialogAction> onClicked = null);

        /// <summary>
        /// Display as a modal Dialog in the specified DialogHost and return immediately
        /// </summary>
        Task ShowAsync(string hostId, Action<IMessageDialog, DialogAction> onClicked = null);

        /// <summary>
        /// Close the dialog regardless of the <see cref="CanClose"/> value
        /// </summary>
        void Close();
    }

    public interface IMessageDialogConfig
    {
        /// <summary>
        /// Create a new instance of <see cref="ITaskDialog"/>.
        /// </summary>
        IMessageDialog Create();

        /// <summary>
        /// Set the title text.
        /// </summary>
        IMessageDialogConfig Title(string title);

        /// <summary>
        /// Set the message text
        /// </summary>
        IMessageDialogConfig Message(string message);

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
        IMessageDialogConfig Icon(IconType iconType = IconType.Custom, ImageReference customIcon = null);

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
}
