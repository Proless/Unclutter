using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using Unclutter.Modules.ViewModels;
using Unclutter.SDK.Dialogs;

namespace Unclutter.ViewModels.Dialogs
{
    public class MessageViewModel : ViewModelBase, IDialogAware
    {
        /* Fields */
        private IDialog _dialog;

        /* Properties */
        public IDialog Dialog
        {
            get => _dialog;
            set => SetProperty(ref _dialog, value);
        }

        /* Commands */
        public DelegateCommand<DialogAction> ActionClickedCommand => new(OnActionClicked);

        /* Methods */
        private void OnActionClicked(DialogAction dialogAction)
        {
            Dialog.OnClicked?.Invoke(Dialog, dialogAction);
            RequestClose?.Invoke(new DialogResult());
        }

        #region IDialogAware
        public bool CanCloseDialog()
        {
            return Dialog.CanClose;
        }
        public void OnDialogClosed()
        {
            Dialog.OnClicked?.Invoke(Dialog, DialogAction.Close);
        }
        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.TryGetValue<IDialog>(LocalIdentifiers.Parameters.Dialog, out var dialog) && dialog != null)
            {
                Dialog = dialog;
            }
        }
        public event Action<IDialogResult> RequestClose;
        #endregion
    }
}
