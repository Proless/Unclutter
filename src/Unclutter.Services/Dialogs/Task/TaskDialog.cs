using MaterialDesignThemes.Wpf;
using System;
using System.Threading.Tasks;
using Unclutter.SDK.Dialogs;
using Unclutter.SDK.Progress;

namespace Unclutter.Services.Dialogs.Task
{
    public class TaskDialog : BaseDialog, ITaskDialog, IProgressModel
    {
        /* Fields */
        private TaskCompletionSource<IProgressController> _tcs;
        private bool _isCancelable;
        private bool _isIndeterminate;
        private bool _autoCancel;
        private double _progressValue;

        /* Properties */
        public bool AutoCancel
        {
            get => _autoCancel;
            set => SetProperty(ref _autoCancel, value);
        }
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
        public IProgressController Controller { get; }

        /* Constructor */
        public TaskDialog()
        {
            Controller = new ProgressController(this);
        }

        /* Methods */
        public Task<IProgressController> ShowDialogAsync(Action<ITaskDialog, DialogAction> onClicked = null)
        {
            if (IsDialogOpen)
            {
                return System.Threading.Tasks.Task.FromResult(Controller);
            }

            OnClicked = (dialog, action) => onClicked?.Invoke(dialog as ITaskDialog, action);

            UIDispatcher.OnUIThread(() =>
            {
                var dlg = new TaskDialogView
                {
                    DataContext = this
                };

                _tcs = new TaskCompletionSource<IProgressController>();
                DialogHost.Show(dlg, OnDialogOpened, OnDialogClosed); // TODO: Fire and Forget!
            });

            return _tcs.Task;
        }
        public Task<IProgressController> ShowDialogAsync(string hostId, Action<ITaskDialog, DialogAction> onClicked = null)
        {
            if (IsDialogOpen)
            {
                return System.Threading.Tasks.Task.FromResult(Controller);
            }

            OnClicked = (dialog, action) => onClicked?.Invoke(dialog as ITaskDialog, action);

            UIDispatcher.OnUIThread(() =>
            {
                var dlg = new TaskDialogView
                {
                    DataContext = this
                };

                _tcs = new TaskCompletionSource<IProgressController>();
                DialogHost.Show(dlg, hostId, OnDialogOpened, OnDialogClosed); // TODO: Fire and Forget!
            });

            return _tcs.Task;
        }
        protected override void OnActionClicked(DialogAction? dialogAction)
        {
            if (dialogAction == DialogAction.Cancel && AutoCancel)
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
