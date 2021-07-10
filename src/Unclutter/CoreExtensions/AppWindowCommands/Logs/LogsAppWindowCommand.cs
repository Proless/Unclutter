using System.Threading.Tasks;
using Unclutter.Modules.Plugins.AppWindowCommand;
using Unclutter.SDK;

namespace Unclutter.CoreExtensions.AppWindowCommands.Logs
{
    [ExportAppWindowCommand]
    public class LogsAppWindowCommand : BaseAppWindowCommand
    {
        public override double? Order { get; }

        protected override Task OnClicked()
        {
            return Task.CompletedTask;
        }

        public override void Initialize()
        {
            Content = new LogsControl();
            Hint = LocalizationProvider.GetLocalizedString(ResourceKeys.Logs);
        }
    }
}
