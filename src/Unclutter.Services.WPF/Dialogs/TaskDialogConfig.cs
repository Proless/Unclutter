using System;
using System.Threading;
using Unclutter.SDK.Dialogs;

namespace Unclutter.Services.WPF.Dialogs
{
    public class TaskDialogConfig : ITaskDialogConfig
    {
        /* Fields */
        private readonly TaskDialog _task;

        /* Constructor */
        public TaskDialogConfig(TaskDialog task)
        {
            _task = task;
        }

        /* Methods */
        public ITaskDialog Create()
        {
            return _task;
        }
        public ITaskDialogConfig Option(string label, bool isChecked = false)
        {
            _task.OptionLabel = label;
            _task.IsOptionChecked = isChecked;
            return this;
        }
        public ITaskDialogConfig OnClicked(Action<ITaskDialog, DialogAction> onClicked)
        {
            _task.OnClicked = (notification, action) => onClicked(notification as ITaskDialog, action);
            return this;
        }
        public ITaskDialogConfig CancelButton(string label, CancellationTokenSource cancellationTokenSource = null)
        {
            _task.CancelButtonLabel = label;
            _task.CancellationTokenSource = cancellationTokenSource;
            _task.IsCancelable = true;
            return this;
        }
        public ITaskDialogConfig IsIndeterminate(bool defaultValue = true)
        {
            _task.IsIndeterminate = defaultValue;
            return this;
        }
        public ITaskDialogConfig CanClose(bool defaultValue = true)
        {
            _task.CanClose = defaultValue;
            return this;
        }
    }
}
