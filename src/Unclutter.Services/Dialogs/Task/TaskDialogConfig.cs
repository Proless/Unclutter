using System.Threading;
using Unclutter.SDK.Common;
using Unclutter.SDK.Dialogs;
using Unclutter.SDK.Images;

namespace Unclutter.Services.Dialogs.Task
{
    public class TaskDialogConfig : ITaskDialogConfig
    {
        /* Fields */
        private readonly TaskDialog _task = new TaskDialog();

        /* Methods */
        public ITaskDialog Create()
        {
            var dlg = new TaskDialog
            {
                Title = _task.Title,
                Message = _task.Message,
                IsOptionChecked = _task.IsOptionChecked,
                OptionLabel = _task.OptionLabel,
                CanClose = _task.CanClose,
                IconType = _task.IconType,
                Icon = _task.Icon,
                AutoCancel = _task.AutoCancel,
                IsCancelable = _task.IsCancelable,
                IsIndeterminate = _task.IsIndeterminate
            };

            dlg.Controller.CancellationTokenSource = _task.Controller.CancellationTokenSource;

            return dlg;
        }
        public ITaskDialogConfig Title(string title)
        {
            _task.Title = title;
            return this;
        }
        public ITaskDialogConfig Message(string message)
        {
            _task.Message = message;
            return this;
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
        public ITaskDialogConfig Icon(IconType iconType = IconType.Custom, ImageReference customIcon = null)
        {
            _task.IconType = iconType;
            _task.Icon = customIcon;
            return this;
        }
        public ITaskDialogConfig CancelButton(CancellationTokenSource cancellationTokenSource = null, bool autoCancel = true)
        {
            _task.IsCancelable = true;
            _task.AutoCancel = autoCancel;
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
