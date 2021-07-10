using Prism.Ioc;
using Prism.Services.Dialogs;
using Unclutter.API.Factory;
using Unclutter.CoreExtensions;
using Unclutter.SDK.App;
using Unclutter.SDK.Data;
using Unclutter.SDK.Dialogs;
using Unclutter.SDK.Notifications;
using Unclutter.SDK.Plugins;
using Unclutter.SDK.Services;
using Unclutter.SDK.Settings;
using Unclutter.Services;
using Unclutter.Services.App;
using Unclutter.Services.Authentication;
using Unclutter.Services.Client;
using Unclutter.Services.Data;
using Unclutter.Services.Dialogs;
using Unclutter.Services.EventAggregator;
using Unclutter.Services.Games;
using Unclutter.Services.Images;
using Unclutter.Services.Localization;
using Unclutter.Services.Logging;
using Unclutter.Services.Notifications;
using Unclutter.Services.Plugins;
using Unclutter.Services.Profiles;
using Unclutter.Services.Settings;
using Unclutter.ViewModels;
using Unclutter.Views.Dialogs;
using Unclutter.Views.ProfilesManagement;

namespace Unclutter
{
    public static class ContainerExtensions
    {
        public static void RegisterRequirements(this IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterViews();
            containerRegistry.RegisterServices();
            containerRegistry.RegisterSingletonServices();
            containerRegistry.RegisterPrismFrameworkExtensions();
            containerRegistry.RegisterShellServices();
        }

        public static void RegisterServices(this IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IDialogProvider, DialogProvider>();
            containerRegistry.Register<IJsonService, JsonService>();
            containerRegistry.Register<IAuthenticationService, AuthenticationService>();
            containerRegistry.Register<IImageProvider, ImageProvider>();
            containerRegistry.Register<IClientServices, ClientServices>();
            containerRegistry.RegisterSingleton<ILoggerProvider, LoggerProvider>();
            containerRegistry.Register<IDirectoryService, DirectoryService>();
        }

        public static void RegisterSingletonServices(this IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterManySingleton<PluginProvider>(typeof(IPluginProvider), typeof(PluginProvider));
            containerRegistry.RegisterInstance<ILocalizationProvider>(LocalizationProvider.Instance);
            containerRegistry.RegisterInstance(LoggerProvider.Instance);

            containerRegistry.RegisterSingleton<IAppNavigator, AppNavigator>();
            containerRegistry.RegisterSingleton<IAppDbProvider, AppDbProvider>();
            containerRegistry.RegisterSingleton<INotificationProvider, NotificationProvider>();
            containerRegistry.RegisterSingleton<IEventAggregator, EventAggregator>();
            containerRegistry.RegisterSingleton<ISqliteDatabaseFactory, SqliteDatabaseFactory>();
            containerRegistry.RegisterSingleton<INexusClientFactory, NexusClientFactory>();
            containerRegistry.RegisterSingleton<IProfilesManager, ProfilesManager>();
            containerRegistry.RegisterSingleton<ISettingsProvider, SettingsProvider>();

            containerRegistry.RegisterSingleton<IGamesProvider, GamesProvider>();
        }

        public static void RegisterPrismFrameworkExtensions(this IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IDialogService, ExtendedDialogService>();

            // Dialog windows
            containerRegistry.RegisterDialogWindow<DialogHostWindow>();
            containerRegistry.RegisterDialogWindow<StartupDialogHostWindow>(LocalIdentifiers.Dialogs.StartupWindow);
        }

        public static void RegisterViews(this IContainerRegistry containerRegistry)
        {
            // Dialogs
            containerRegistry.RegisterDialog<StartupDialogView>(LocalIdentifiers.Dialogs.Startup);

            // Profiles management
            containerRegistry.RegisterForNavigation<ProfilesView>(LocalIdentifiers.Views.Profiles);
            containerRegistry.RegisterForNavigation<ProfilesCreationView>(LocalIdentifiers.Views.ProfilesCreation);
            containerRegistry.RegisterForNavigation<GamesSelectionView>(LocalIdentifiers.Views.GamesSelection);
            containerRegistry.RegisterForNavigation<AuthenticationView>(LocalIdentifiers.Views.Authentication);
        }

        public static void RegisterShellServices(this IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterManySingleton<ShellViewModel>(typeof(ShellViewModel), typeof(IAppWindowServices));
        }
    }
}
