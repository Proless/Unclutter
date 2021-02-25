using System.Threading;
using System.Threading.Tasks;
using Unclutter.SDK.Notifications;

namespace Unclutter.Services.WPF.Notifications
{
    internal class TaskNotification : BaseNotification, ITaskNotification
    {
        /* Fields */
        private bool _isCancelable;
        private bool _isIndeterminate;
        private double _progressValue;

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

        /* Constructor */
        public TaskNotification()
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
