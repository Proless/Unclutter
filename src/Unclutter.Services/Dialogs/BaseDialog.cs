using MaterialDesignThemes.Wpf;
using Prism.Commands;
using System;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Unclutter.Modules.ViewModels;
using Unclutter.SDK.Dialogs;

namespace Unclutter.Services.Dialogs
{
    public abstract class BaseDialog : ViewModelBase
    {
        protected DialogSession DialogSession;

        /* Fields */
        private string _text;
        private bool _canClose;
        private bool _isOptionChecked;
        private object _icon;
        private string _optionLabel;

        /* Commands */
        public DelegateCommand<DialogAction?> ActionClickedCommand => new DelegateCommand<DialogAction?>(OnActionClicked);

        /* Properties */
        public bool IsDialogOpen { get; protected set; }
        public Action<BaseDialog, DialogAction> OnClicked { get; protected set; }
        public DialogIcon IconType { get; internal set; }
        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }
        public object Icon
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
            IconType = DialogIcon.Custom;
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
            try
            {
                if (forceClose)
                {
                    CanClose = true;
                }
                DialogSession?.Close();
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
            if (IconType != DialogIcon.Custom)
            {
                var resourceName = GetIconResourceName();
                if (resourceName == string.Empty)
                {
                    return;
                }

                var assembly = Assembly.GetExecutingAssembly();

                var imageName = assembly
                    .GetManifestResourceNames()
                    .Single(r => r.EndsWith(resourceName));

                var icon = assembly.GetManifestResourceStream(imageName);

                var imgSource = new BitmapImage();
                imgSource.BeginInit();
                imgSource.StreamSource = icon;
                imgSource.CacheOption = BitmapCacheOption.OnLoad;
                imgSource.EndInit();
                imgSource.Freeze();

                Icon = new Image
                {
                    Source = imgSource
                };

                RenderOptions.SetBitmapScalingMode((Image)Icon, BitmapScalingMode.HighQuality);
                RenderOptions.SetEdgeMode((Image)Icon, EdgeMode.Aliased);
            }
        }

        private string GetIconResourceName()
        {
            switch (IconType)
            {
                case DialogIcon.Information:
                    return "information.png";
                case DialogIcon.Question:
                    return "question.png";
                case DialogIcon.Warning:
                    return "warning.png";
                case DialogIcon.Error:
                    return "error.png";
                case DialogIcon.Task:
                    return "task.png";
                default:
                    return string.Empty;
            }
        }
    }
}
