using MaterialDesignThemes.Wpf;
using System;
using System.Threading.Tasks;
using Unclutter.SDK.Dialogs;

namespace Unclutter.Services.Dialogs.Message
{
    public class MessageDialog : BaseDialog, IMessageDialog
    {
        /* Fields */
        private TaskCompletionSource _tcs;
        private string _leftButtonLabel;
        private string _midButtonLabel;
        private string _rightButtonLabel;

        /* Properties */
        public string LeftButtonLabel
        {
            get => _leftButtonLabel;
            set => SetProperty(ref _leftButtonLabel, value);
        }
        public string MidButtonLabel
        {
            get => _midButtonLabel;
            set => SetProperty(ref _midButtonLabel, value);
        }
        public string RightButtonLabel
        {
            get => _rightButtonLabel;
            set => SetProperty(ref _rightButtonLabel, value);
        }

        /* Methods */
        public System.Threading.Tasks.Task ShowDialogAsync(Action<IMessageDialog, DialogAction> onClicked = null)
        {
            if (IsDialogOpen)
            {
                return System.Threading.Tasks.Task.CompletedTask;
            }

            OnClicked = (dialog, action) => onClicked?.Invoke(dialog as IMessageDialog, action);

            return UIDispatcher.OnUIThreadAsync(() =>
            {
                var dlg = new MessageDialogView
                {
                    DataContext = this
                };

                _tcs = new TaskCompletionSource();

                return DialogHost.Show(dlg, OnDialogOpened, OnDialogClosed);
            });
        }
        public System.Threading.Tasks.Task ShowDialogAsync(string hostId, Action<IMessageDialog, DialogAction> onClicked = null)
        {
            if (IsDialogOpen)
            {
                return System.Threading.Tasks.Task.CompletedTask;
            }

            OnClicked = (dialog, action) => onClicked?.Invoke(dialog as IMessageDialog, action);

            return UIDispatcher.OnUIThreadAsync(() =>
            {
                var dlg = new MessageDialogView
                {
                    DataContext = this
                };

                _tcs = new TaskCompletionSource();

                return DialogHost.Show(dlg, hostId, OnDialogOpened, OnDialogClosed);
            });
        }
        public System.Threading.Tasks.Task ShowAsync(Action<IMessageDialog, DialogAction> onClicked = null)
        {
            if (IsDialogOpen)
            {
                return System.Threading.Tasks.Task.CompletedTask;
            }

            OnClicked = (dialog, action) => onClicked?.Invoke(dialog as IMessageDialog, action);

            return UIDispatcher.BeginOnUIThread(() =>
            {
                var dlg = new MessageDialogView
                {
                    DataContext = this
                };

                _tcs = new TaskCompletionSource();

                DialogHost.Show(dlg, OnDialogOpened, OnDialogClosed); // TODO: Fire and Forget!
            }).ContinueWith(_ => _tcs.Task);
        }
        public System.Threading.Tasks.Task ShowAsync(string hostId, Action<IMessageDialog, DialogAction> onClicked = null)
        {
            if (IsDialogOpen)
            {
                return System.Threading.Tasks.Task.CompletedTask;
            }

            OnClicked = (dialog, action) => onClicked?.Invoke(dialog as IMessageDialog, action);

            return UIDispatcher.BeginOnUIThread(() =>
            {
                var dlg = new MessageDialogView
                {
                    DataContext = this
                };

                _tcs = new TaskCompletionSource();

                DialogHost.Show(dlg, hostId, OnDialogOpened, OnDialogClosed); // TODO: Fire and Forget!
            }).ContinueWith(_ => _tcs.Task);
        }
        protected override void OnDialogOpened(object sender, DialogOpenedEventArgs args)
        {
            base.OnDialogOpened(sender, args);
            _tcs.SetResult();
        }
    }
}
