using MaterialDesignThemes.Wpf;
using System;
using System.Threading.Tasks;
using Unclutter.SDK.Common;
using Unclutter.SDK.Dialogs;

namespace Unclutter.Services.Dialogs.TaskDlg
{
    public class TaskDialog : BaseDialog, ITaskDialog
    {
        /* Fields */
        private TaskCompletionSource<IProgressController> _tcs;
        private bool _isCancelable;
        private bool _isIndeterminate;
        private double _progressValue;
        private string _cancelButtonLabel;

        /* Properties */
        public bool IsCancelable
        {
            get => _isCancelable;
            set => SetProperty(ref _isCancelable, value);
        }
        public bool IsIndeterminate
        {
            get => _isIndeterminate;
            set => SetProperty(ref _isIndeterminate, value);
        }
        public double ProgressValue
        {
            get => _progressValue;
            set => SetProperty(ref _progressValue, value);
        }
        public string CancelButtonLabel
        {
            get => _cancelButtonLabel;
            set => SetProperty(ref _cancelButtonLabel, value);
        }
        public IProgressController Controller { get; }

        /* Constructor */
        public TaskDialog()
        {
            Controller = new TaskDialogProgressController(this);
        }

        /* Methods */
        public Task<IProgressController> ShowDialogAsync(Action<ITaskDialog, DialogAction> onClicked = null)
        {
            if (IsDialogOpen)
            {
                return Task.FromResult(Controller);
            }

            OnClicked = (dialog, action) => onClicked?.Invoke(dialog as ITaskDialog, action);
            var dlg = new TaskDialogView
            {
                DataContext = this
            };

            _tcs = new TaskCompletionSource<IProgressController>();
            DialogHost.Show(dlg, OnDialogOpened, OnDialogClosed);

            return _tcs.Task;
        }

        public Task<IProgressController> ShowDialogAsync(string hostId, Action<ITaskDialog, DialogAction> onClicked = null)
        {
            if (IsDialogOpen)
            {
                return Task.FromResult(Controller);
            }

            OnClicked = (dialog, action) => onClicked?.Invoke(dialog as ITaskDialog, action);
            var dlg = new TaskDialogView
            {
                DataContext = this
            };

            _tcs = new TaskCompletionSource<IProgressController>();
            DialogHost.Show(dlg, hostId, OnDialogOpened, OnDialogClosed);

            return _tcs.Task;
        }

        protected override void OnActionClicked(DialogAction? dialogAction)
        {
            if (dialogAction == DialogAction.Cancel)
            {
                Controller.CancellationTokenSource?.Cancel();
            }
            base.OnActionClicked(dialogAction);
        }

        protected override void OnDialogOpened(object sender, DialogOpenedEventArgs args)
        {
            base.OnDialogOpened(sender, args);
            _tcs.SetResult(Controller);
        }
    }
}
