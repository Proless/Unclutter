using MaterialDesignThemes.Wpf;
using Unclutter.SDK;
using Unclutter.SDK.Common;
using Unclutter.SDK.Plugins;
using Unclutter.Theme;
using Unclutter.Theme.Services;

namespace Unclutter.CoreExtensions.StartupActions
{
    [ExportStartupAction]
    public class NewProfileStartupAction : BaseStartupAction
    {
        public override double? Order => null;

        public override void Initialize()
        {
            Label = LocalizationProvider.GetLocalizedString(ResourceKeys.Profile_New);
            Hint = LocalizationProvider.GetLocalizedString(ResourceKeys.Profile_New_Hint);
            Icon = new MaterialDesignIconImageReference(PackIconKind.Plus);
        }

        protected override void OnClicked()
        {
            RegionManager.RequestNavigate(CommonIdentifiers.Regions.Startup, LocalIdentifiers.Views.ProfilesCreation);
        }
    }
}
