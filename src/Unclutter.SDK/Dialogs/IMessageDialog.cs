using System;
using System.Threading.Tasks;

namespace Unclutter.SDK.Dialogs
{
    public interface IMessageDialog
    {
        /// <summary>
        /// Determines if the option checkbox is checked
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
        /// Close the dialog regardless of the <see cref="CanClose"/> value
        /// </summary>
        void Close();
    }
}
