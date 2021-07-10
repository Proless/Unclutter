using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Unclutter.Modules.Plugins.AppWindowCommand;
using Unclutter.SDK;
using Unclutter.SDK.App;

namespace Unclutter.CoreExtensions.AppWindowCommands.Settings
{
    [ExportAppWindowCommand]
    public class SettingsAppWindowCommand : BaseAppWindowCommand
    {
        /* Fields */
        private readonly IAppWindowServices _appWindowServices;

        /* Properties */
        public override double? Order => double.MinValue;

        /* Constructor */
        [ImportingConstructor]
        public SettingsAppWindowCommand(IAppWindowServices appWindowServices)
        {
            _appWindowServices = appWindowServices;
        }

        /* Methods */
        protected override Task OnClicked()
        {
            _appWindowServices.OpenFlyout(LocalIdentifiers.Flyouts.Settings);
            return Task.CompletedTask;
        }
        public override void Initialize()
        {
            Content = new SettingsControl();
            Hint = LocalizationProvider.GetLocalizedString(ResourceKeys.Settings);
        }
    }
}
