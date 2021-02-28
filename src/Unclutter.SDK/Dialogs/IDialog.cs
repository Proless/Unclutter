using System;
using Unclutter.SDK.Common;

namespace Unclutter.SDK.Dialogs
{
    public interface IDialog
    {
        /// <summary>
        /// Determines whether a dialog should be closed after <see cref="OnClicked"/> action is executed, default is true
        /// </summary>
        bool CanClose { get; set; }

        /// <summary>
        /// Determines whether the check-box was checked when <see cref="OnClicked"/> got invoked
        /// </summary>
        bool IsOptionChecked { get; }

        /// <summary>
        /// The action to execute when a button is clicked on the Dialog
        /// </summary>
        Action<IDialog, DialogAction> OnClicked { get; }

        /// <summary>
        /// The type of the Dialog
        /// </summary>
        DialogType Type { get; }

        /// <summary>
        /// Open a non modal dialog
        /// </summary>
        void Show();

        /// <summary>
        /// Open a modal dialog
        /// </summary>
        void ShowDialog();

        /// <summary>
        /// Closes the dialog regardless of the <see cref="CanClose"/> property value
        /// </summary>
        void Close();
    }

    public interface ITaskDialog : IDialog, IProgressController { }
}
