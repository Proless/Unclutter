using System.ComponentModel.Composition;
using Prism.Ioc;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace Unclutter
{
    public class MefServicesFactory
    {
        [Export]
        public IRegionManager RegionManager => ContainerLocator.Current.Resolve<IRegionManager>();

        [Export]
        public IDialogService DialogService => ContainerLocator.Current.Resolve<IDialogService>();
    }
}
