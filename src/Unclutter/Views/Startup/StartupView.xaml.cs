using System.Windows.Controls;
using Prism.Regions;

namespace Unclutter.Views.Startup
{
    /// <summary>
    /// Interaction logic for StartupView.xaml
    /// </summary>
    public partial class StartupView : UserControl
    {
        public StartupView(IRegionManager regionManager)
        {
            InitializeComponent();
            RegionManager.SetRegionName(Transitioner, LocalIdentifiers.Regions.Startup);
            RegionManager.SetRegionManager(Transitioner, regionManager);
        }
    }
}
