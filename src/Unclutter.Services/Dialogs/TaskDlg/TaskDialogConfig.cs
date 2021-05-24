using System.Threading;
using Unclutter.SDK.Dialogs;

namespace Unclutter.Services.Dialogs.TaskDlg
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
        public ITaskDialogConfig CanClose(bool defaultValue = true)
        {
            _task.CanClose = defaultValue;
            return this;
        }
        public ITaskDialogConfig Icon(DialogIcon iconType = DialogIcon.Custom, object customIcon = null)
        {
            _task.IconType = iconType;
            _task.Icon = customIcon;
            return this;
        }
        public ITaskDialogConfig CancelButton(string label, CancellationTokenSource cancellationTokenSource)
        {
            _task.CancelButtonLabel = label;
            _task.IsCancelable = true;
            _task.Controller.CancellationTokenSource = cancellationTokenSource;
            return this;
        }
        public ITaskDialogConfig IsIndeterminate(bool defaultValue = true)
        {
            _task.IsIndeterminate = defaultValue;
            return this;
        }
    }
}
