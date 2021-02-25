using System;
using Unclutter.SDK.Dialogs;

namespace Unclutter.Services.WPF.Dialogs
{
    internal class DialogConfig : BaseDialogConfig, IDialogConfig
    {
        /* Fields */
        private readonly Dialog _dialog;

        /* Event */
        public override event Action<BaseDialog> Created;

        /* Constructor */
        public DialogConfig(Dialog dialog)
        {
            _dialog = dialog;
        }

        /* Methods */
        public IDialog Build()
        {
            var newDlg = Create();
            Created?.Invoke(newDlg);
            return newDlg;
        }
        public IDialogConfig OnClicked(Action<IDialog, DialogAction> onClicked)
        {
            _dialog.OnClicked = onClicked;
            return this;
        }
        public IDialogConfig Option(string label, bool isChecked = false)
        {
            _dialog.IsOptionChecked = isChecked;
            _dialog.OptionLabel = label;
            return this;
        }
        public IDialogConfig LeftButton(string label)
        {
            _dialog.LeftButtonLabel = label;
            return this;
        }
        public IDialogConfig MidButton(string label)
        {
            _dialog.MidButtonLabel = label;
            return this;
        }
        public IDialogConfig RightButton(string label)
        {
            _dialog.RightButtonLabel = label;
            return this;
        }
        public IDialogConfig CanClose(bool defaultValue = true)
        {
            _dialog.CanClose = defaultValue;
            return this;
        }

        /* Helpers */
        private Dialog Create()
        {
            return new Dialog
            {
                Text = _dialog.Text,
                Title = _dialog.Title,
                Type = _dialog.Type,
                IsOptionChecked = _dialog.IsOptionChecked,
                CanClose = _dialog.CanClose,
                OnClicked = _dialog.OnClicked,
                OptionLabel = _dialog.OptionLabel,
                Icon = _dialog.Icon,
                LeftButtonLabel = _dialog.LeftButtonLabel,
                MidButtonLabel = _dialog.MidButtonLabel,
                RightButtonLabel = _dialog.RightButtonLabel
            };
        }
    }
}
