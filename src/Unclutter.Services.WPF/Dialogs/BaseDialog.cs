using Prism.Mvvm;
using System;
using Unclutter.SDK.Dialogs;

namespace Unclutter.Services.WPF.Dialogs
{
    internal abstract class BaseDialog : BindableBase, IDialog
    {
        /* Fields */
        private string _title;
        private string _text;
        private bool _canClose;
        private bool _isOptionChecked;
        private object _icon;

        /* Event */
        public event Action<BaseDialog> RequestClose;
        public event Action<BaseDialog, OpenMode> RequestOpen;

        /* Properties */
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }
        public object Icon
        {
            get => _icon;
            set => SetProperty(ref _icon, value);
        }

        /* Constructor */
        internal BaseDialog()
        {
            Title = null;
            Text = null;
            CanClose = true;
            IsOptionChecked = false;
        }

        #region IDialog
        public bool CanClose
        {
            get => _canClose;
            set => SetProperty(ref _canClose, value);
        }
        public bool IsOptionChecked
        {
            get => _isOptionChecked;
            set => SetProperty(ref _isOptionChecked, value);
        }
        public Action<IDialog, DialogAction> OnClicked { get; set; }
        public DialogType Type { get; set; }
        public void Show()
        {
            RequestOpen?.Invoke(this, OpenMode.NonModal);
        }
        public void ShowDialog()
        {
            RequestOpen?.Invoke(this, OpenMode.Modal);
        }
        public void Close()
        {
            RequestClose?.Invoke(this);
        }
        #endregion
    }
}
