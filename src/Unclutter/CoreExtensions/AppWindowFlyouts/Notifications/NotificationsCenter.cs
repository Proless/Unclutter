using Prism.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Unclutter.Modules.Commands;
using Unclutter.Modules.Plugins.AppWindowBar;
using Unclutter.Modules.Plugins.AppWindowFlyout;
using Unclutter.SDK.Notifications;
using Unclutter.Services;

namespace Unclutter.CoreExtensions.AppWindowFlyouts.Notifications
{
    [ExportAppWindowFlyout]
    [Export(typeof(INotificationCenter))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NotificationsCenter : BaseAppWindowFlyout, INotificationCenter
    {
        /* Fields */
        private readonly INotificationProvider _notificationProvider;
        private int _selectedNotificationTabIndex;

        /* Properties */
        public override string Name => LocalIdentifiers.Flyouts.Notifications;
        public ReadOnlyObservableCollection<INotification> Notifications => _notificationProvider.Notifications;
        public ReadOnlyObservableCollection<ITaskNotification> Tasks => _notificationProvider.Tasks;
        public int SelectedNotificationTabIndex
        {
            get => _selectedNotificationTabIndex;
            set => SetProperty(ref _selectedNotificationTabIndex, value);
        }

        /* Commands */
        public DelegateCommand CloseAllCommand => new AsyncDelegateCommand(CloseAll);

        /* Constructor */
        [ImportingConstructor]
        public NotificationsCenter(INotificationProvider notificationProvider)
        {
            _notificationProvider = notificationProvider;
        }

        /* Methods */
        public override void Initialize()
        {
            Position = AppWindowFlyoutPosition.Right;
            Content = new NotificationCenterView
            {
                DataContext = this
            };
        }
        public override void OnClosed()
        {

        }
        public override void OnOpened()
        {

        }
        public Task CloseAll()
        {
            return UIDispatcher.OnUIThreadAsync(() =>
           {
               _notificationProvider.CloseAll(NotificationType.Message);
           });
        }
        void INotificationCenter.Open()
        {
            IsOpen = true;
        }
        void INotificationCenter.Open(NotificationType type)
        {
            IsOpen = true;
            SelectedNotificationTabIndex = type == NotificationType.Message ? 0 : 1;
        }
        void INotificationCenter.Close()
        {
            IsOpen = false;
        }
    }
}
