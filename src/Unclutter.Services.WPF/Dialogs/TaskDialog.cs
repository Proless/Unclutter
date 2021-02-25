using System.Threading;
using System.Threading.Tasks;
using Unclutter.SDK.Dialogs;

namespace Unclutter.Services.WPF.Dialogs
{
    internal class TaskDialog : BaseDialog, ITaskDialog
    {
        /* Fields */
        private bool _isCancelable;
        private bool _isIndeterminate;
        private double _progressValue;
        private string _cancelButtonLabel;
        private string _optionLabel;

        /* Properties */
        public bool IsCancelable
        {
            get => _isCancelable;
            set => SetProperty(ref _isCancelable, value);
        }
        public bool IsIndeterminate
        {
            get => _isIndeterminate;
            set => SetProperty(ref _isIndeterminate, value);
        }
        public double ProgressValue
        {
            get => _progressValue;
            set => SetProperty(ref _progressValue, value);
        }
        public string CancelButtonLabel
        {
            get => _cancelButtonLabel;
            set => SetProperty(ref _cancelButtonLabel, value);
        }
        public string OptionLabel
        {
            get => _optionLabel;
            set => SetProperty(ref _optionLabel, value);
        }

        /* Constructor */
        public TaskDialog()
        {
            IsCancelable = false;
            IsIndeterminate = true;
            ProgressValue = 0d;
        }

        #region IProgressController
        public CancellationTokenSource CancellationTokenSource { get; set; }
        public void SetCancelable(bool cancelable)
        {
            IsCancelable = cancelable;
        }
        public void SetIndeterminate(bool indeterminate)
        {
            IsIndeterminate = indeterminate;
        }
        public void SetText(string text)
        {
            Text = text;
        }
        public void SetProgress(double progress)
        {
            if (progress >= 0d || progress <= 100d)
            {
                SetIndeterminate(false);
                ProgressValue = progress;
            }
        }
        public void SetTitle(string title)
        {
            Title = title;
        }
        public Task CloseAsync()
        {
            return Task.Run(Close);
        }
        #endregion
    }
}
