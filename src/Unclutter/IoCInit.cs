using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Unclutter.API.Factory;
using Unclutter.Initialization;
using Unclutter.Modules;
using Unclutter.SDK.Data;
using Unclutter.SDK.Dialogs;
using Unclutter.SDK.IServices;
using Unclutter.SDK.Plugins;
using Unclutter.SDK.Settings;
using Unclutter.Services;
using Unclutter.Services.Authentication;
using Unclutter.Services.Data;
using Unclutter.Services.Dialogs;
using Unclutter.Services.EventAggregator;
using Unclutter.Services.Games;
using Unclutter.Services.Images;
using Unclutter.Services.Loader;
using Unclutter.Services.Localization;
using Unclutter.Services.Logging;
using Unclutter.Services.Notifications;
using Unclutter.Services.Plugins;
using Unclutter.Services.Profiles;
using Unclutter.Services.Settings;
using Unclutter.Views;
using Unclutter.Views.Dialogs;
using Unclutter.Views.ProfilesManagement;

namespace Unclutter
{
    public static class IoCInit
    {
        public static void RegisterRequirements(this IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterViews();
            containerRegistry.RegisterServices();
            containerRegistry.RegisterSingletonServices();
            containerRegistry.RegisterPrismFrameworkExtensions();
        }

        public static void RegisterServices(this IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IDialogProvider, DialogProvider>();
            containerRegistry.Register<IJsonService, JsonService>();

            containerRegistry.Register<IAuthenticationService, AuthenticationService>();
            containerRegistry.Register<IDialogProvider, DialogProvider>();
            containerRegistry.Register<IGamesProvider, GamesProvider>();
            containerRegistry.Register<IImageProvider, ImageProvider>();
        }

        public static void RegisterSingletonServices(this IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ILoaderService, LoaderService>();
            containerRegistry.Register<IDirectoryService, DirectoryService>();
            containerRegistry.RegisterInstance<ILocalizationProvider>(LocalizationProvider.Instance);

            containerRegistry.RegisterSingleton<IModuleProvider, ModuleProvider>();
            containerRegistry.RegisterSingleton<ILoggerProvider, LoggerProvider>();
            containerRegistry.RegisterSingleton<IPluginProvider, PluginProvider>();
            containerRegistry.RegisterSingleton<IEventAggregator, EventAggregator>();

            containerRegistry.RegisterSingleton<INotificationService, NotificationService>();

            containerRegistry.RegisterSingleton<ISqliteDatabaseFactory, SqliteDatabaseFactory>();
            containerRegistry.RegisterSingleton<INexusClientFactory, NexusClientFactory>();

            containerRegistry.RegisterSingleton<IProfilesManager, ProfilesManager>();
            containerRegistry.RegisterSingleton<ISettingsService, SettingsService>();
        }

        public static void RegisterPrismFrameworkExtensions(this IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IModuleInitializer, ModuleInitializerProcessor>();
            containerRegistry.RegisterDialogWindow<DialogWindow>();

            // !!!
            containerRegistry.Register<RegionNavigationService, RegionNavigationService>();
            containerRegistry.Register<IRegionNavigationService, CustomRegionNavigationService>();

            // !!!
            containerRegistry.Register<RegionNavigationJournal, RegionNavigationJournal>();
            containerRegistry.Register<IRegionNavigationJournal, CustomRegionNavigationJournal>();
        }

        public static void RegisterViews(this IContainerRegistry containerRegistry)
        {
            // Dialogs
            containerRegistry.RegisterDialog<StartupDialogView>(LocalIdentifiers.Dialogs.Startup);

            // Startup
            containerRegistry.RegisterForNavigation<StartupView>(LocalIdentifiers.Views.Startup);
            containerRegistry.RegisterForNavigation<StartupActionsView>(LocalIdentifiers.Views.StartupActions);

            // Profiles management
            containerRegistry.RegisterForNavigation<ProfilesView>(LocalIdentifiers.Views.Profiles);
            containerRegistry.RegisterForNavigation<ProfilesCreationView>(LocalIdentifiers.Views.ProfilesCreation);
            containerRegistry.RegisterForNavigation<GamesSelectionView>(LocalIdentifiers.Views.GamesSelection);
            containerRegistry.RegisterForNavigation<AuthenticationView>(LocalIdentifiers.Views.Authentication);
        }
    }
}
