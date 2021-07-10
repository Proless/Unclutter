using System.Collections.Generic;

namespace Unclutter.SDK.App
{
    public interface IAppNavigator
    {
        IEnumerable<AppView> AppViews { get; }
        IEnumerable<SettingsView> SettingsViews { get; }

        void RegisterAppView(params AppView[] views);
        void RegisterSettingsView(params SettingsView[] views);
    }
}
