using System.Threading.Tasks;
using Unclutter.Modules.Plugins;
using Unclutter.SDK;
using Unclutter.Services.Localization;

namespace Unclutter.Initialization.ShellCommandActions.Notifications
{
    public class NotificationsShellCommandAction : ShellCommandActionBase
    {
        protected override Task ExecuteAction()
        {
            return Task.CompletedTask;
        }

        public override void Initialize()
        {
            Control = new NotificationsControl();
            Hint = LocalizationProvider.Instance.GetLocalizedString(ResourceKeys.Notifications);
        }
    }
}
