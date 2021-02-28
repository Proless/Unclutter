using Prism.Commands;
using Prism.Mvvm;
using System;
using System.ComponentModel;
using System.Windows;
using Unclutter.SDK.Dialogs;

namespace Unclutter.Services.WPF.Dialogs
{
    public abstract class BaseDialog : BindableBase, IDialog
    {
        /* Fields */
        private readonly Window _dlgWindow;
        private string _title;
        private string _text;
        private bool _canClose;
        private bool _isOptionChecked;
        private object _icon;
        private string _optionLabel;

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
        public string OptionLabel
        {
            get => _optionLabel;
            set => SetProperty(ref _optionLabel, value);
        }

        /* Commands */
        public DelegateCommand<DialogAction?> ActionClickedCommand => new(OnActionClicked);

        /* Constructor */
        internal BaseDialog(Window dlgWindow)
        {
            _dlgWindow = dlgWindow ?? throw new ArgumentNullException(nameof(dlgWindow));
            _dlgWindow.DataContext = this;
            _dlgWindow.Closing += OnClosing;
            Title = null;
            Text = null;
            CanClose = true;
            IsOptionChecked = false;
        }

        /* Methods */
        public void Show()
        {
            Application.Current.Dispatcher.Invoke(() => _dlgWindow.Show());
        }
        public void ShowDialog()
        {
            Application.Current.Dispatcher.Invoke(() => _dlgWindow.ShowDialog());
        }
        public void Close()
        {
            Application.Current.Dispatcher.Invoke(() => _dlgWindow.Close());
        }

        /* Helpers */
        protected virtual void OnActionClicked(DialogAction? dialogAction)
        {
            if (dialogAction is null) return;
            OnClicked?.Invoke(this, (DialogAction)dialogAction);

            if (CanClose)
            {
                Close();
            }
        }
        private void OnClosing(object sender, CancelEventArgs e)
        {
            OnClicked?.Invoke(this, DialogAction.Close);
            if (!CanClose)
            {
                e.Cancel = true;
            }
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
        #endregion
    }
}
