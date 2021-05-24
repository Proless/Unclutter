using Prism.Commands;
using Prism.Regions;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Unclutter.SDK;
using Unclutter.SDK.Plugins;
using Unclutter.Services.Localization;

namespace Unclutter.Initialization.StartupActions
{
    [ExportStartupAction]
    public class NewProfileStartupAction : IStartupAction
    {
        /* Fields */
        private readonly IRegionManager _regionManager;

        /* Properties */
        public string Label => LocalizationProvider.Instance.GetLocalizedString(ResourceKeys.Profile_New);
        public string IconRef => "PlusBoxMultiple";
        public string Hint => LocalizationProvider.Instance.GetLocalizedString(ResourceKeys.Profile_New_Hint);
        public double Priority => 0.1D;
        public ICommand Action => new DelegateCommand(Execute);

        /* Constructor */
        [ImportingConstructor]
        public NewProfileStartupAction(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        /* Methods */
        public void Execute()
        {
            _regionManager.RequestNavigate(LocalIdentifiers.Regions.ProfilesManagement, LocalIdentifiers.Views.ProfilesCreation);
        }

    }
}
