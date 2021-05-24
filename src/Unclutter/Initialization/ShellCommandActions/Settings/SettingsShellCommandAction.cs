using System.Threading.Tasks;
using Unclutter.Modules.Plugins;
using Unclutter.SDK;
using Unclutter.Services.Localization;

namespace Unclutter.Initialization.ShellCommandActions.Settings
{
    public class SettingsShellCommandAction : ShellCommandActionBase
    {
        protected override Task ExecuteAction()
        {
            return Task.CompletedTask;
        }

        public override void Initialize()
        {
            Control = new SettingsControl();
            Hint = LocalizationProvider.Instance.GetLocalizedString(ResourceKeys.Settings);
            Priority = 0.133d;
        }
    }
}
