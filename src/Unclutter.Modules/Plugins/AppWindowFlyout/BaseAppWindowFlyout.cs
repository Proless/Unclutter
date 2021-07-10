using Prism.Mvvm;
using System.Windows;
using Unclutter.Modules.Plugins.AppWindowBar;

namespace Unclutter.Modules.Plugins.AppWindowFlyout
{
    public abstract class BaseAppWindowFlyout : BindableBase, IAppWindowFlyout
    {
        /* Fields */
        private bool _isOpen;
        private bool _isModal;
        private bool _isPinned;
        private bool _showCloseButton;
        private object _content;
        private object _title;
        private AppWindowFlyoutPosition _position;
        private HorizontalAlignment _horizontalContentAlignment;
        private VerticalAlignment _verticalContentAlignment;

        /* Properties */
        public HorizontalAlignment HorizontalContentAlignment
        {
            get => _horizontalContentAlignment;
            set => SetProperty(ref _horizontalContentAlignment, value);
        }
        public VerticalAlignment VerticalContentAlignment
        {
            get => _verticalContentAlignment;
            set => SetProperty(ref _verticalContentAlignment, value);
        }
        public AppWindowFlyoutPosition Position
        {
            get => _position;
            protected set => SetProperty(ref _position, value);
        }
        public bool ShowCloseButton
        {
            get => _showCloseButton;
            set => SetProperty(ref _showCloseButton, value);
        }
        public bool IsPinned
        {
            get => _isPinned;
            protected set => SetProperty(ref _isPinned, value);
        }
        public bool IsModal
        {
            get => _isModal;
            protected set => SetProperty(ref _isModal, value);
        }
        public bool IsOpen
        {
            get => _isOpen;
            set => SetProperty(ref _isOpen, value);
        }
        public object Content
        {
            get => _content;
            protected set => SetProperty(ref _content, value);
        }
        public object Title
        {
            get => _title;
            protected set => SetProperty(ref _title, value);
        }

        /* Constructor*/
        protected BaseAppWindowFlyout()
        {
            ShowCloseButton = false;
            IsPinned = false;
            IsModal = true;
            IsOpen = false;
            Content = null;
            Title = null;
            Position = AppWindowFlyoutPosition.Left;
            HorizontalContentAlignment = HorizontalAlignment.Center;
            VerticalContentAlignment = VerticalAlignment.Center;
        }

        /* Abstract */
        public abstract string Name { get; }
        public abstract void Initialize();
        public abstract void OnClosed();
        public abstract void OnOpened();
    }
}
