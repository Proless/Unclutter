using MaterialDesignThemes.Wpf;
using Unclutter.SDK;
using Unclutter.SDK.Common;
using Unclutter.SDK.Plugins;
using Unclutter.Theme;
using Unclutter.Theme.Services;

namespace Unclutter.CoreExtensions.StartupActions
{
    [ExportStartupAction]
    public class ProfilesStartupAction : BaseStartupAction
    {
        public override double? Order => double.MinValue;

        public override void Initialize()
        {
            Label = LocalizationProvider.GetLocalizedString(ResourceKeys.Profiles);
            Hint = LocalizationProvider.GetLocalizedString(ResourceKeys.Profiles);
            Icon = new MaterialDesignIconImageReference(PackIconKind.AccountBoxMultiple);
        }

        protected override void OnClicked()
        {
            RegionManager.RequestNavigate(CommonIdentifiers.Regions.Startup, LocalIdentifiers.Views.Profiles);
        }
    }
}
