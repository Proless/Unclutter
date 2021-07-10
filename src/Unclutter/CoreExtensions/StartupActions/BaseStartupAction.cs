using Prism.Commands;
using Prism.Regions;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Unclutter.SDK.Images;
using Unclutter.SDK.Plugins;
using Unclutter.SDK.Services;

namespace Unclutter.CoreExtensions.StartupActions
{
    public abstract class BaseStartupAction : IStartupAction
    {
        /* Services */
        [Import]
        public ILocalizationProvider LocalizationProvider { get; protected set; }
        [Import]
        public IRegionManager RegionManager { get; protected set; }

        /* Properties */
        public abstract double? Order { get; }
        public string Label { get; set; }
        public ImageReference Icon { get; set; }
        public string Hint { get; set; }
        public virtual ICommand Action => new DelegateCommand(OnClicked);

        /* Methods */
        public abstract void Initialize();
        protected abstract void OnClicked();
    }
}
