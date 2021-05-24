using MaterialDesignThemes.Wpf;
using System;
using System.Threading.Tasks;
using Unclutter.SDK.Dialogs;

namespace Unclutter.Services.Dialogs.Message
{
    public class MessageDialog : BaseDialog, IMessageDialog
    {
        /* Fields */
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
        public Task ShowDialogAsync(Action<IMessageDialog, DialogAction> onClicked = null)
        {
            if (IsDialogOpen)
            {
                return Task.CompletedTask;
            }

            OnClicked = (dialog, action) => onClicked?.Invoke(dialog as IMessageDialog, action);
            var dlg = new MessageDialogView
            {
                DataContext = this
            };

            return DialogHost.Show(dlg, OnDialogOpened, OnDialogClosed);
        }

        public Task ShowDialogAsync(string hostId, Action<IMessageDialog, DialogAction> onClicked = null)
        {
            if (IsDialogOpen)
            {
                return Task.CompletedTask;
            }

            OnClicked = (dialog, action) => onClicked?.Invoke(dialog as IMessageDialog, action);
            var dlg = new MessageDialogView
            {
                DataContext = this
            };

            return DialogHost.Show(dlg, hostId, OnDialogOpened, OnDialogClosed);
        }
    }
}
