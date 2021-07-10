using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using System;
using Unclutter.SDK.Common;
using Unclutter.SDK.Dialogs;
using Unclutter.SDK.Images;

namespace Unclutter.Services.Dialogs
{
    public abstract class BaseDialog : BindableBase
    {
        protected DialogSession DialogSession;

        /* Fields */
        private string _message;
        private bool _canClose;
        private bool _isOptionChecked;
        private ImageReference _icon;
        private string _optionLabel;
        private string _title;

        /* Commands */
        public DelegateCommand<DialogAction?> ActionClickedCommand => new DelegateCommand<DialogAction?>(OnActionClicked);

        /* Properties */
        public bool IsDialogOpen { get; private set; }
        public Action<BaseDialog, DialogAction> OnClicked { get; protected set; }
        public IconType IconType { get; internal set; }
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
        public ImageReference Icon
        {
            get => _icon;
            internal set => SetProperty(ref _icon, value);
        }
        public string OptionLabel
        {
            get => _optionLabel;
            set => SetProperty(ref _optionLabel, value);
        }
        public bool IsOptionChecked
        {
            get => _isOptionChecked;
            set => SetProperty(ref _isOptionChecked, value);
        }
        public bool CanClose
        {
            get => _canClose;
            set => SetProperty(ref _canClose, value);
        }

        /* Constructor */
        protected BaseDialog()
        {
            IsDialogOpen = false;
            CanClose = true;
            IconType = IconType.Custom;
        }

        /* Methods */
        public void Close()
        {
            InternalClose(true);
        }

        /* Helpers */
        protected virtual void OnActionClicked(DialogAction? dialogAction)
        {
            if (dialogAction is null) throw new ArgumentNullException(nameof(dialogAction), "The value of the passed in dialog action was null, this should never happen!!!");
            OnClicked?.Invoke(this, (DialogAction)dialogAction);
            InternalClose();
        }

        protected virtual void InternalClose(bool forceClose = false)
        {
            if (!IsDialogOpen) return;

            try
            {
                if (forceClose)
                {
                    CanClose = true;
                }
                UIDispatcher.OnUIThread(DialogSession.Close);
            }
            catch (InvalidOperationException)
            {
                // ignore
            }
        }

        protected virtual void OnDialogClosed(object sender, DialogClosingEventArgs args)
        {
            if (!CanClose)
            {
                args.Cancel();
            }
            else
            {
                IsDialogOpen = false;
            }
        }

        protected virtual void OnDialogOpened(object sender, DialogOpenedEventArgs args)
        {
            IsDialogOpen = true;
            SetIcon();
            DialogSession = args.Session;
        }

        protected virtual void SetIcon()
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
