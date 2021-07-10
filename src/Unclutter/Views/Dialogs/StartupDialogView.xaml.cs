using Prism.Regions;
using System.Windows.Controls;
using Unclutter.SDK.Common;

namespace Unclutter.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for StartupDialogView.xaml
    /// </summary>
    public partial class StartupDialogView : UserControl
    {
        public StartupDialogView(IRegionManager regionManager)
        {
            InitializeComponent();
            RegionManager.SetRegionName(StartupRegion, CommonIdentifiers.Regions.Startup);
            RegionManager.SetRegionManager(StartupRegion, regionManager);
        }
    }
}
