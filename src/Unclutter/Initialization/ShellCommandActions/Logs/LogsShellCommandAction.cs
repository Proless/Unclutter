using System.Threading.Tasks;
using Unclutter.Modules.Plugins;
using Unclutter.SDK;
using Unclutter.Services.Localization;

namespace Unclutter.Initialization.ShellCommandActions.Logs
{
    public class LogsShellCommandAction : ShellCommandActionBase
    {
        protected override Task ExecuteAction()
        {
            return Task.CompletedTask;
        }

        public override void Initialize()
        {
            Control = new LogsControl();
            Hint = LocalizationProvider.Instance.GetLocalizedString(ResourceKeys.Logs);
        }
    }
}
