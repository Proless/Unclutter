using System;
using System.Threading.Tasks;
using Unclutter.SDK.Common;

namespace Unclutter.SDK.Dialogs
{
    public interface ITaskDialog
    {
        /// <summary>
        /// Get a controller object to report progress
        /// </summary>
        IProgressController Controller { get; }

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
}
