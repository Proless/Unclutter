using Unclutter.SDK.Dialogs;

namespace Unclutter.Services.Dialogs.Message
{
    public class MessageDialogConfig : IMessageDialogConfig
    {
        /* Fields */
        private readonly MessageDialog _messageDialog;

        /* Constructor */
        public MessageDialogConfig(MessageDialog messageDialog)
        {
            _messageDialog = messageDialog;
        }

        /* Methods */
        public IMessageDialog Create()
        {
            return _messageDialog;
        }
        public IMessageDialogConfig Option(string label, bool isChecked = false)
        {
            _messageDialog.IsOptionChecked = isChecked;
            _messageDialog.OptionLabel = label;
            return this;
        }
        public IMessageDialogConfig LeftButton(string label)
        {
            _messageDialog.LeftButtonLabel = label;
            return this;
        }
        public IMessageDialogConfig MidButton(string label)
        {
            _messageDialog.MidButtonLabel = label;
            return this;
        }
        public IMessageDialogConfig RightButton(string label)
        {
            _messageDialog.RightButtonLabel = label;
            return this;
        }
        public IMessageDialogConfig CanClose(bool defaultValue = true)
        {
            _messageDialog.CanClose = defaultValue;
            return this;
        }
        public IMessageDialogConfig Icon(DialogIcon iconType = DialogIcon.Custom, object customIcon = null)
        {
            _messageDialog.IconType = iconType;
            _messageDialog.Icon = customIcon;
            return this;
        }
    }
}
