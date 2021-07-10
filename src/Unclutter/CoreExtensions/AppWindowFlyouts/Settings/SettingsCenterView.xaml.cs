using Prism.Regions;
using System.Windows.Controls;

namespace Unclutter.CoreExtensions.AppWindowFlyouts.Settings
{
    /// <summary>
    /// Interaction logic for SettingsCenterView.xaml
    /// </summary>
    public partial class SettingsCenterView : UserControl
    {
        public SettingsCenterView(IRegionManager regionManager)
        {
            InitializeComponent();
            RegionManager.SetRegionManager(SettingsRegionControl, regionManager);
            RegionManager.SetRegionName(SettingsRegionControl, LocalIdentifiers.Regions.Settings);
        }
    }
}
