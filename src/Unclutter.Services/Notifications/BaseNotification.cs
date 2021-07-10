using Prism.Commands;
using Prism.Mvvm;
using System;
using Unclutter.SDK.Common;
using Unclutter.SDK.Images;
using Unclutter.SDK.Notifications;

namespace Unclutter.Services.Notifications
{
    public abstract class BaseNotification : BindableBase, INotification
    {
        /* Fields */
        private ImageReference _icon;
        private string _message;
        private string _actionLabel;
        private string _title;

        /* Internals */
        internal Action<BaseNotification> CloseRequested { get; set; }
        internal Action<BaseNotification> ShowRequested { get; set; }
        internal virtual void OnShowed()
        {
            SetIcon();
            IsShown = true;
        }
        internal virtual void OnClosed()
        {
            if (!IsShown) return;

            IsShown = false;
            Closed?.Invoke();
        }

        /* Commands */
        public DelegateCommand ActionClickedCommand => new DelegateCommand(OnActionClicked);
        public DelegateCommand CloseCommand => new DelegateCommand(Close);

        /* Properties */
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        public string ActionLabel
        {
            get => _actionLabel;
            set => SetProperty(ref _actionLabel, value);
        }
        public ImageReference Icon
        {
            get => _icon;
            internal set => SetProperty(ref _icon, value);
        }
        public IconType IconType { get; internal set; }
        public bool IsShown { get; protected set; }
        public DateTimeOffset Created { get; }
        public NotificationType Type { get; protected set; }
        public Action ActionClicked { get; protected set; }
        public Action Closed { get; protected set; }

        /* Constructors */
        protected BaseNotification()
        {
            Created = DateTimeOffset.Now;
            IconType = IconType.Custom;
            Type = NotificationType.Message;
            IsShown = false;
        }

        /* Methods */
        public void Show(Action onActionClicked = null, Action onClosed = null)
        {
            if (IsShown) return;

            ActionClicked = onActionClicked;
            Closed = onClosed;

            ShowRequested?.Invoke(this);
        }
        public void Close()
        {
            if (!IsShown) return;
            CloseRequested?.Invoke(this);
        }
        protected virtual void OnActionClicked()
        {
            ActionClicked?.Invoke();
        }

        /* Helpers */
        private void SetIcon()
        {
            if (IconType == IconType.Custom) return;

            var iconFileName = GetIconFileName();
            if (iconFileName == string.Empty)
            {
                return;
            }

            Icon = new ResourceImageReference($"Resources/{iconFileName}");
        }
        private string GetIconFileName()
        {
            return IconType switch
            {
                IconType.Information => "information.png",
                IconType.Question => "question.png",
                IconType.Warning => "warning.png",
                IconType.Error => "error.png",
                IconType.Task => "task.png",
                _ => string.Empty
            };
        }
    }
}
