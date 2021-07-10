using Prism.Ioc;
using System.Collections.Generic;
using Unclutter.SDK.App;

namespace Unclutter.Services.App
{
    public class AppNavigator : IAppNavigator
    {
        /* Services */
        private readonly IContainerExtension _containerExtension;

        /* Fields */
        private readonly List<AppView> _appViews = new List<AppView>();
        private readonly List<SettingsView> _settingsViews = new List<SettingsView>();

        /* Properties */
        public IEnumerable<AppView> AppViews => _appViews;
        public IEnumerable<SettingsView> SettingsViews => _settingsViews;

        /* Constructor */
        public AppNavigator(IContainerExtension containerExtension)
        {
            _containerExtension = containerExtension;
        }

        /* Methods */
        public void RegisterAppView(params AppView[] views)
        {
            if (views == null) return;

            foreach (var view in views)
            {
                _containerExtension.RegisterForNavigation(view.ViewType, view.Identifier);
                _appViews.Add(view);
            }
        }
        public void RegisterSettingsView(params SettingsView[] views)
        {
            if (views == null) return;

            foreach (var view in views)
            {
                _containerExtension.RegisterForNavigation(view.ViewType, view.Identifier);
                _settingsViews.Add(view);
            }
        }
    }
}
