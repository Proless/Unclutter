using Prism.Regions;
using System.Windows.Controls;

namespace Unclutter.Views.ProfilesManagement
{
    /// <summary>
    /// Interaction logic for ProfilesCreationView.xaml
    /// </summary>
    public partial class ProfilesCreationView : UserControl
    {
        public ProfilesCreationView(IRegionManager regionManager)
        {
            InitializeComponent();
            regionManager.Regions.Remove(LocalIdentifiers.Regions.Games);
            regionManager.Regions.Remove(LocalIdentifiers.Regions.Authentication);

            RegionManager.SetRegionName(GamesRegion, LocalIdentifiers.Regions.Games);
            RegionManager.SetRegionManager(GamesRegion, regionManager);

            RegionManager.SetRegionName(AuthenticationRegion, LocalIdentifiers.Regions.Authentication);
            RegionManager.SetRegionManager(AuthenticationRegion, regionManager);
        }
    }
}
