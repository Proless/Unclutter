using System;
using Prism.Mvvm;
using Unclutter.SDK.Notifications;

namespace Unclutter.Services.Notifications
{
    internal abstract class BaseNotification : BindableBase, INotification
    {
        /* Fields */
        private string _title;
        private string _text;
        private bool _canClose;
        private bool _isOptionChecked;

        /* Event */
        public event Action<BaseNotification> Closed;

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


        /* Constructor */
        internal BaseNotification()
        {
            Title = null;
            Text = null;
            CanClose = true;
            IsOptionChecked = false;
        }

        #region INotification
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
        public Action<INotification, NotificationAction> OnClicked { get; set; }
        public NotificationType Type { get; set; }
        public void Close()
        {
            Closed?.Invoke(this);
        }
        #endregion
    }
}
