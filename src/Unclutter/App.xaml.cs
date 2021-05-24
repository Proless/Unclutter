using DryIoc;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Unclutter.AppInstance;
using Unclutter.Initialization;
using Unclutter.SDK;
using Unclutter.SDK.Common;
using Unclutter.SDK.IServices;
using Unclutter.SDK.Loader;
using Unclutter.SDK.Plugins;
using Unclutter.Services.Container;
using Unclutter.Services.Loader;
using Unclutter.Services.Localization;
using Unclutter.Services.Logging;
using Unclutter.Services.Plugins;
using Unclutter.Views;

namespace Unclutter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication, ILoader
    {
        public static string LoadingMessage => LocalizationProvider.Instance.GetLocalizedString(ResourceKeys.Loading);

        /* Properties */
        public LoaderScreenManager LoaderScreenManager { get; }
        public ILoaderService LoaderService { get; private set; }
        public IPluginProvider PluginProvider { get; private set; }
        public ILogger Logger { get; private set; }
        public bool IsInitialized { get; private set; }

        /* Constructor */
        public App()
        {
            AppInstanceManager.VerifyApplicationInstance();

            SetupExceptionHandling();

            LocalizationProvider.Instance.Configure();

            LoaderScreenManager = LoaderScreenManager.Create<LoaderScreen>(s =>
            {
                s.Closed += (_, _) =>
                {
                    if (IsInitialized) return;

                    Logger.Warning("Application closed during initialization !");
                    Environment.Exit(-1);
                };
                s.Title = AppInfo.Name;
                s.Subtitle = $"v{AppInfo.Version}";
                s.Footer = "Proless";

                var logoImage = new BitmapImage(new Uri("../Resources/logo.png", UriKind.Relative));

                s.Logo = new Image
                {
                    Stretch = Stretch.Uniform,
                    Source = logoImage,
                    Height = 120
                };
            });
        }

        // 0 - Setup IoC Container
        protected override IContainerExtension CreateContainerExtension()
        {
            var containerExtensions = new DryIocContainerExtension(new Container(CreateContainerRules()));
            var mefContainerExtensions = new DryIocAdapterContainerExtensions(containerExtensions, containerExtensions);

            mefContainerExtensions.RegisterInstance<IContainerExportProvider>(new ContainerExportProvider(mefContainerExtensions));

            return mefContainerExtensions;
        }

        // 1
        protected override void OnStartup(StartupEventArgs e)
        {
            LoaderScreenManager.Show();
            LoaderScreenManager.ReportProgress(LoadingMessage);
            base.OnStartup(e);
        }

        // 3.1
        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new AggregateDirectoryModuleCatalog(".\\extensions");
        }

        // 3.3
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterRequirements();
        }

        // 3.7
        protected override Window CreateShell()
        {
            // Delay shell creation.
            return null;
        }

        // 3.9
        protected override void InitializeModules()
        {
            // Delay modules initialization.
        }

        // 4
        protected override async void OnInitialized()
        {
            try
            {
                await Setup();
            }
            catch (Exception ex)
            {
                LogUnhandledException(ex, "Setup");
                LoaderScreenManager.ReportProgress(new ProgressReport(string.Format(LocalizationProvider.Instance.GetLocalizedString(ResourceKeys.App_Startup_Error), Logger.Location), 0d, OperationStatus.Error));
                await Task.Delay(TimeSpan.FromSeconds(15));
                Shutdown(-1);
            }
        }

        public async void OnNewArguments(string[] args)
        {
            if (!IsInitialized) return;

            var handlers = PluginProvider.Container.GetExportedValues<ICommandLineArgumentsHandler>().OrderBy(h => h.Priority);
            foreach (var argsHandler in handlers)
            {
                await argsHandler.HandleAsync(args);
            }
        }

        /* Helpers */
        private async Task Setup()
        {
            // configure Prism
            ViewModelLocationProvider.SetDefaultViewModelFactory((view, type) =>
            {
                var viewmodel = Container.Resolve(type);

                foreach (var processor in PluginProvider.Container.GetExportedValues<IViewModelProcessor>())
                {
                    try
                    {
                        processor.ProcessViewModel(viewmodel, view);
                    }
                    catch (Exception ex)
                    {
                        Logger?.Error(ex, "Exception while processing ViewModel: {ViewModelType}, ViewModelProcessor: {ViewModelProcessorType}", viewmodel.GetType().ToString(), processor.GetType().ToString());
                    }
                }
                return viewmodel;
            });

            // services
            PluginProvider = Container.Resolve<IPluginProvider>();
            LoaderService = Container.Resolve<ILoaderService>();

            // add event handlers
            LoaderService.ProgressChanged += LoaderScreenManager.ReportProgress;
            LoaderService.Finished += OnFinishedLoading;

            // initialize MEF / plugins
            await LoaderService.Load(PluginProvider);

            RegisterLoaders();

            // initialize modules
            base.InitializeModules();

            // must be the last to register
            LoaderService.RegisterLoader(this);

            // load
            await LoaderService.Load();
        }

        private void DelayedInitializeShell()
        {
            var shell = Container.Resolve<ShellView>();
            RegionManager.SetRegionManager(shell, Container.Resolve<IRegionManager>());
            RegionManager.UpdateRegions();
            MainWindow = shell;
        }

        private void OnFinishedLoading()
        {
            LoaderService.ProgressChanged -= LoaderScreenManager.ReportProgress;
            LoaderService.Finished -= OnFinishedLoading;
            LoaderScreenManager.Close();
            MainWindow?.Show();
            MainWindow?.Activate();
        }

        private void SetupExceptionHandling()
        {
            Logger = LoggerProvider.Instance;

            AppDomain.CurrentDomain.UnhandledException += (_, e) =>
                LogUnhandledException((Exception)e.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");

            DispatcherUnhandledException += (_, e) =>
            {
                LogUnhandledException(e.Exception, "Application.Current.DispatcherUnhandledException");
                e.Handled = true;
            };

            TaskScheduler.UnobservedTaskException += (_, e) =>
            {
                LogUnhandledException(e.Exception, "TaskScheduler.UnobservedTaskException");
                e.SetObserved();
            };
        }

        private void LogUnhandledException(Exception exception, string source)
        {
            var message = $"Unhandled exception ({source})";
            try
            {
                var assemblyName = Assembly.GetExecutingAssembly().GetName();
                message = $"Unhandled exception in {assemblyName.Name} v{assemblyName.Version}";
            }
            catch (Exception ex)
            {
                Logger?.Error(ex, "Exception in LogUnhandledException");
            }
            finally
            {
                Logger?.Error(exception, message);
            }
        }

        private void RegisterLoaders()
        {
            LoaderService.RegisterLoader(Container.Resolve<IDirectoryService>());
        }

        #region ILoader
        public event Action<ProgressReport> ProgressChanged;
        public LoadOptions LoaderOptions => new LoadOptions() { LoadThread = ThreadOption.UIThread };
        public Task Load()
        {
            ProgressChanged?.Invoke(new ProgressReport(LoadingMessage));

            DelayedInitializeShell();

            // configure AppInstanceManager
            AppInstanceManager.SetupApplicationInstance();
            AppInstanceManager.NewArguments += OnNewArguments;

            IsInitialized = true;

            LoaderScreenManager.Hide();

            Container.Resolve<IDialogService>().ShowDialog(LocalIdentifiers.Dialogs.Startup, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    LoaderScreenManager.Show();
                    LoaderService.RegisterLoader(MainWindow?.DataContext as ILoader);
                }
                else
                {
                    Environment.Exit(0);
                }
            });

            return Task.CompletedTask;
        }
        #endregion
    }
}
