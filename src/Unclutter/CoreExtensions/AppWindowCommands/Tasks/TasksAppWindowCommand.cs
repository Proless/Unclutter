using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Unclutter.Modules.Plugins.AppWindowCommand;
using Unclutter.SDK;
using Unclutter.SDK.Notifications;

namespace Unclutter.CoreExtensions.AppWindowCommands.Tasks
{
    [ExportAppWindowCommand]
    public class TasksAppWindowCommand : BaseAppWindowCommand
    {
        /* Fields */
        private readonly INotificationCenter _notificationCenter;
        private readonly INotificationProvider _notificationProvider;
        private int? _count;

        /* Properties */
        public int? Count
        {
            get => _count;
            set => SetProperty(ref _count, value);
        }
        public override double? Order { get; }

        /* Constructor */
        [ImportingConstructor]
        public TasksAppWindowCommand(INotificationCenter notificationCenter, INotificationProvider notificationProvider)
        {
            _notificationCenter = notificationCenter;
            _notificationProvider = notificationProvider;
            _notificationProvider.TasksChanged += OnTasksChanged;

            OnTasksChanged();
        }

        /* Methods */
        protected override Task OnClicked()
        {
            if (_notificationCenter.IsOpen)
            {
                _notificationCenter.Close();
            }
            else
            {
                _notificationCenter.Open(NotificationType.Task);
            }
            return Task.CompletedTask;
        }
        public override void Initialize()
        {
            Content = new TasksControl
            {
                DataContext = this
            };
            Hint = LocalizationProvider.GetLocalizedString(ResourceKeys.Tasks);
        }
        private void OnTasksChanged()
        {
            if (_notificationProvider.Tasks.Count == 0)
            {
                Count = null;
            }
            else
            {
                Count = _notificationProvider.Tasks.Count;
            }
        }
    }
}
