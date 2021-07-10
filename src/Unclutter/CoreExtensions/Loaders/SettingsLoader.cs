using MaterialDesignThemes.Wpf;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Unclutter.SDK.App;
using Unclutter.SDK.Plugins;
using Unclutter.SDK.Settings;
using Unclutter.Theme.Services;
using Unclutter.Views.Settings;

namespace Unclutter.CoreExtensions.Loaders
{
    [ExportLoader]
    public class SettingsLoader : BaseLoader
    {
        /* Fields */
        private readonly IAppNavigator _appNavigator;
        private readonly ISettingsProvider _settingsProvider;

        [ImportingConstructor]
        public SettingsLoader(IAppNavigator appNavigator, ISettingsProvider settingsProvider)
        {
            _appNavigator = appNavigator;
            _settingsProvider = settingsProvider;
        }

        /* Methods */
        public override Task Load()
        {
            var settings = _settingsProvider.RegisterSettings<AppSettings>("app");

            var generalSettings = new SettingsView(typeof(GeneralSettingsView), "General", settings) { Icon = new MaterialDesignIconImageReference(PackIconKind.Cog) };
            var downloadsSettings = new SettingsView(typeof(GeneralSettingsView), "Downloads", settings) { Icon = new MaterialDesignIconImageReference(PackIconKind.Downloads) };

            var networkSettingsGroup = new GroupSettingsView(typeof(GeneralSettingsView), "Network", settings) { Icon = new MaterialDesignIconImageReference(PackIconKind.Network) };
            var proxySettings = new SettingsView(typeof(GeneralSettingsView), "Proxy", settings) { Icon = new MaterialDesignIconImageReference(PackIconKind.Proxy) };
            var internetSettings = new SettingsView(typeof(GeneralSettingsView), "Internet", settings) { Icon = new MaterialDesignIconImageReference(PackIconKind.MicrosoftInternetExplorer) };
            
            var networkSettingsSubGroup = new GroupSettingsView(typeof(GeneralSettingsView), "Network-Sub", settings) { Icon = new MaterialDesignIconImageReference(PackIconKind.NetworkFavorite) };
            var internetSettingsSub = new SettingsView(typeof(GeneralSettingsView), "Internet-Sub-Sub", settings) { Icon = new MaterialDesignIconImageReference(PackIconKind.NetworkInterfaceCard) };
            networkSettingsSubGroup.AddSubView(internetSettingsSub);

            networkSettingsGroup.AddSubView(proxySettings);
            networkSettingsGroup.AddSubView(internetSettings);
            networkSettingsGroup.AddSubView(networkSettingsSubGroup);

            var pluginsSettingsGroup = new SettingsView(typeof(GeneralSettingsView), "Plugins", settings) { Icon = new MaterialDesignIconImageReference(PackIconKind.Plugin) };

            _appNavigator.RegisterSettingsView(generalSettings, downloadsSettings, networkSettingsGroup, pluginsSettingsGroup);

            return Task.CompletedTask;
        }
    }
}
