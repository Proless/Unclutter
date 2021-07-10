using Unclutter.SDK.Common;
using Unclutter.SDK.Dialogs;
using Unclutter.SDK.Images;

namespace Unclutter.Services.Dialogs.Message
{
    public class MessageDialogConfig : IMessageDialogConfig
    {
        /* Fields */
        private readonly MessageDialog _messageDialog = new MessageDialog();

        /* Methods */
        public IMessageDialog Create()
        {
            return new MessageDialog
            {
                Title = _messageDialog.Title,
                Message = _messageDialog.Message,
                IsOptionChecked = _messageDialog.IsOptionChecked,
                OptionLabel = _messageDialog.OptionLabel,
                CanClose = _messageDialog.CanClose,
                IconType = _messageDialog.IconType,
                Icon = _messageDialog.Icon,
                LeftButtonLabel = _messageDialog.LeftButtonLabel,
                MidButtonLabel = _messageDialog.MidButtonLabel,
                RightButtonLabel = _messageDialog.RightButtonLabel
            };
        }
        public IMessageDialogConfig Title(string title)
        {
            _messageDialog.Title = title;
            return this;
        }
        public IMessageDialogConfig Message(string message)
        {
            _messageDialog.Message = message;
            return this;
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
        public IMessageDialogConfig Icon(IconType iconType = IconType.Custom, ImageReference customIcon = null)
        {
            _messageDialog.IconType = iconType;
            _messageDialog.Icon = customIcon;
            return this;
        }
    }
}
