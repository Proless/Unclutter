using System;
using System.Threading;
using Unclutter.SDK.Dialogs;

namespace Unclutter.Services.WPF.Dialogs
{
    internal class TaskDialogConfig : BaseDialogConfig, ITaskDialogConfig
    {
        /* Fields */
        private readonly TaskDialog _task;

        /* Event */
        public override event Action<BaseDialog> Created;

        /* Constructor */
        public TaskDialogConfig(TaskDialog task)
        {
            _task = task;
        }

        /* Methods */
        public ITaskDialog Build()
        {
            var newDlg = Create();
            Created?.Invoke(newDlg);
            return newDlg;
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

        /* Helpers */
        private TaskDialog Create()
        {
            return new TaskDialog
            {
                Text = _task.Text,
                Title = _task.Title,
                Type = _task.Type,
                IsOptionChecked = _task.IsOptionChecked,
                CanClose = _task.CanClose,
                CancelButtonLabel = _task.CancelButtonLabel,
                Icon = _task.Icon,
                OnClicked = _task.OnClicked,
                CancellationTokenSource = _task.CancellationTokenSource,
                IsCancelable = _task.IsCancelable,
                IsIndeterminate = _task.IsIndeterminate,
                OptionLabel = _task.OptionLabel,
                ProgressValue = _task.ProgressValue
            };
        }
    }
}
