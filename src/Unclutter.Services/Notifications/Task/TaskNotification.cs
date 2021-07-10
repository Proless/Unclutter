using Unclutter.SDK.Notifications;
using Unclutter.SDK.Progress;

namespace Unclutter.Services.Notifications.Task
{
    public class TaskNotification : BaseNotification, ITaskNotification, IProgressModel
    {
        /* Fields */
        private bool _isCancelable;
        private bool _isIndeterminate;
        private bool _autoCancel;
        private double _progressValue;

        /* Properties */
        public bool AutoCancel
        {
            get => _autoCancel;
            set => SetProperty(ref _autoCancel, value);
        }
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
        public IProgressController Controller { get; set; }

        /* Constructor */
        public TaskNotification()
        {
            Type = NotificationType.Task;
            Controller = new ProgressController(this);
            AutoCancel = true;
            IsIndeterminate = true;
            ProgressValue = 0d;
        }

        /* Methods */
        protected override void OnActionClicked()
        {
            if (AutoCancel)
            {
                Controller.CancellationTokenSource?.Cancel();
            }
            base.OnActionClicked();
            base.Close();
        }
    }
}
