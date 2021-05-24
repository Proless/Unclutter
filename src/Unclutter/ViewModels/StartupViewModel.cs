using Prism.Regions;
using Unclutter.Modules.ViewModels;

namespace Unclutter.ViewModels
{
    public class StartupViewModel : ViewModelBase
    {
        /* Methods */
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            RegionManager.RequestNavigate(LocalIdentifiers.Regions.StartupActions, LocalIdentifiers.Views.StartupActions);
            RegionManager.RequestNavigate(LocalIdentifiers.Regions.ProfilesManagement, LocalIdentifiers.Views.Profiles);
        }
    }
}
