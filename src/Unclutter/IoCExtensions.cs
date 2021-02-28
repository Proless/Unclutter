using Prism.Ioc;
using Unclutter.Modules;
using Unclutter.NexusAPI;
using Unclutter.SDK.IServices;
using Unclutter.Services;
using Unclutter.Services.Authentication;
using Unclutter.Services.Data;
using Unclutter.Services.Games;
using Unclutter.Services.Logging;
using Unclutter.Services.ModelMapper;
using Unclutter.Services.Plugins;
using Unclutter.Services.Profiles;
using Unclutter.Services.WPF;
using Unclutter.Services.WPF.Dialogs;
using Unclutter.Services.WPF.Images;
using Unclutter.Services.WPF.Notifications;
using Unclutter.Theme;
using Unclutter.Views.Startup;

namespace Unclutter
{
    public static class IoCExtensions
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
            containerRegistry.Register<IJSONService, JSONService>();
            containerRegistry.Register<IAuthenticationService, AuthenticationService>();
            containerRegistry.Register<IImageProvider, ImageProvider>();
            containerRegistry.Register<IDirectoryService, DirectoryService>();
            containerRegistry.Register<IThemeProvider, ThemeProvider>();
            containerRegistry.Register<IDialogProvider, DialogProvider>();
        }

        public static void RegisterSingletonServices(this IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IGamesProvider, GamesProvider>();
            containerRegistry.RegisterSingleton<IProfilesManager, ProfilesManager>();
            containerRegistry.RegisterManySingleton<ProfileProvider>(typeof(IProfileProvider), typeof(IProfileInstanceProvider));
            containerRegistry.RegisterSingleton<IModelMapper, ModelMapper>();
            containerRegistry.RegisterSingleton<IModuleProvider, ModuleProvider>();
            containerRegistry.RegisterSingleton<IAppDatabaseProvider, AppDatabaseProvider>();
            containerRegistry.RegisterSingleton<IPluginsProvider, MefPluginsProvider>();
            containerRegistry.RegisterSingleton<INotificationService, NotificationService>();
            containerRegistry.RegisterSingleton<INexusAPIClientFactory, NexusAPIClientFactory>();
            containerRegistry.RegisterSingleton<ILoggerProvider, LoggerProvider>();
        }

        public static void RegisterPrismFrameworkExtensions(this IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialogWindow<MetroDialogWindow>();
        }

        public static void RegisterViews(this IContainerRegistry containerRegistry)
        {
            // Dialogs
            containerRegistry.RegisterDialog<StartupView>(LocalIdentifiers.Dialogs.Startup);

            //// Views
            //containerRegistry.RegisterForNavigation<StartupView>(LocalIdentifiers.Views.Startup);
            //containerRegistry.RegisterForNavigation<StartupActionsView>(LocalIdentifiers.Views.StartupActions);
            //containerRegistry.RegisterForNavigation<ProfilesView>(LocalIdentifiers.Views.Profiles);
            //containerRegistry.RegisterForNavigation<NewProfileView>(LocalIdentifiers.Views.ProfileCreation);
            //containerRegistry.RegisterForNavigation<GameSelectionView>(LocalIdentifiers.Views.GameSelection);
            //containerRegistry.RegisterForNavigation<NotificationCenterView>(LocalIdentifiers.Views.NotificationCenter);
            //containerRegistry.RegisterForNavigation<SettingsView>(LocalIdentifiers.Views.Settings);
        }
    }
}
