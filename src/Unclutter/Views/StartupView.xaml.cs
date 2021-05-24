using Prism.Regions;
using System.Windows.Controls;

namespace Unclutter.Views
{
    /// <summary>
    /// Interaction logic for StartupView.xaml
    /// </summary>
    public partial class StartupView : UserControl
    {
        public StartupView(IRegionManager regionManager)
        {
            InitializeComponent();
            RegionManager.SetRegionName(ProfilesManagementRegion, LocalIdentifiers.Regions.ProfilesManagement);
            RegionManager.SetRegionManager(ProfilesManagementRegion, regionManager);
            RegionManager.SetRegionName(StartupActionsRegion, LocalIdentifiers.Regions.StartupActions);
            RegionManager.SetRegionManager(StartupActionsRegion, regionManager);
        }
    }
}
