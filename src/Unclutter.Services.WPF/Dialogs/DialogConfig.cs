using System;
using Unclutter.SDK.Dialogs;

namespace Unclutter.Services.WPF.Dialogs
{
    public class DialogConfig : IDialogConfig
    {
        /* Fields */
        private readonly Dialog _dialog;

        /* Constructor */
        public DialogConfig(Dialog dialog)
        {
            _dialog = dialog;
        }

        /* Methods */
        public IDialog Create()
        {
            return _dialog;
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
    }
}
