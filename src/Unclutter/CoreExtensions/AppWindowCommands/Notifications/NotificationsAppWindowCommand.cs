using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Unclutter.Modules.Plugins.AppWindowCommand;
using Unclutter.SDK;
using Unclutter.SDK.Notifications;

namespace Unclutter.CoreExtensions.AppWindowCommands.Notifications
{
    [ExportAppWindowCommand]
    public class NotificationsAppWindowCommand : BaseAppWindowCommand
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
        public NotificationsAppWindowCommand(INotificationCenter notificationCenter, INotificationProvider notificationProvider)
        {
            _notificationCenter = notificationCenter;
            _notificationProvider = notificationProvider;
            _notificationProvider.NotificationsChanged += OnNotificationsChanged;

            OnNotificationsChanged();
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
                _notificationCenter.Open(NotificationType.Message);
            }
            return Task.CompletedTask;
        }
        public override void Initialize()
        {
            Content = new NotificationsControl
            {
                DataContext = this
            };
            Hint = LocalizationProvider.GetLocalizedString(ResourceKeys.Notifications);
        }
        private void OnNotificationsChanged()
        {
            if (_notificationProvider.Notifications.Count == 0)
            {
                Count = null;
            }
            else
            {
                Count = _notificationProvider.Notifications.Count;
            }
        }
    }
}
