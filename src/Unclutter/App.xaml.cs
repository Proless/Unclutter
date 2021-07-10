using DryIoc;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using Unclutter.AppInstance;
using Unclutter.CoreExtensions;
using Unclutter.Modules;
using Unclutter.SDK;
using Unclutter.SDK.Common;
using Unclutter.SDK.Images;
using Unclutter.SDK.Plugins;
using Unclutter.SDK.Progress;
using Unclutter.SDK.Services;
using Unclutter.Services.Container;
using Unclutter.Services.Dialogs;
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
        public LoaderService LoaderService { get; private set; }
        public PluginProvider PluginProvider { get; private set; }
        public ILogger Logger { get; private set; }
        public bool IsInitialized { get; private set; }
        public double? Order => null;

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
                s.Logo = new PackUriImageReference("/Unclutter.Theme;component/Resources/logo.png");
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

        // 4
        protected override async void OnInitialized()
        {
            try
            {
                await Setup();
            }
            catch (Exception ex)
            {
                LogUnhandledException(ex, nameof(Setup));
                LoaderScreenManager.ReportProgress(new ProgressReport(LocalizationProvider.Instance.GetLocalizedString(ResourceKeys.App_Startup_Error, Logger.Location), 0d, OperationStatus.Error));
                await Task.Delay(TimeSpan.FromSeconds(30));
                Environment.Exit(-1);
            }
        }

        /* Helpers */
        private async Task Setup()
        {
            // Configure VM Factory
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

            // Core Services
            PluginProvider = Container.Resolve<PluginProvider>();
            LoaderService = Container.Resolve<LoaderService>();

            // Add event handlers
            LoaderService.LoadProgressed += LoaderScreenManager.ReportProgress;
            LoaderService.LoadFinished += OnFinishedLoading;

            // Initialize core services !! Order is important!!
            PluginProvider.Initialize();
            LoaderService.Initialize();

            // This will execute after all exported ILoader.
            LoaderService.RegisterLoader(this);

            // Start loading.
            await LoaderService.Load();
        }

        private void DelayedCreateShell()
        {
            var shell = Container.Resolve<ShellView>();
            RegionManager.SetRegionManager(shell, Container.Resolve<IRegionManager>());
            RegionManager.UpdateRegions();
            MainWindow = shell;
        }

        private void OnFinishedLoading()
        {
            LoaderService.LoadProgressed -= LoaderScreenManager.ReportProgress;
            LoaderService.LoadFinished -= OnFinishedLoading;
            MainWindow?.Show();
            MainWindow?.Activate();
            LoaderScreenManager.Close();
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

        private void OnNewInstanceStarted(string[] args)
        {
            if (!IsInitialized) return;

            if (Current.MainWindow != null)
            {
                if (Current.MainWindow.WindowState == WindowState.Minimized)
                    Current.MainWindow.WindowState = WindowState.Normal;

                Current.MainWindow.Activate();

                Win32.SetForegroundWindow(new WindowInteropHelper(Current.MainWindow).Handle);
            }

            var handlers = OrderHelper.GetOrdered(PluginProvider.Container.GetExportedValues<ICommandLineArgumentsHandler>());

            foreach (var argsHandler in handlers)
            {
                argsHandler.Handle(args);
            }
        }

        #region ILoader
        public event Action<ProgressReport> ProgressChanged;
        public LoadOptions LoaderOptions => new LoadOptions() { LoadThread = ThreadOption.UIThread };
        public Task Load()
        {
            ProgressChanged?.Invoke(new ProgressReport(LoadingMessage));

            DelayedCreateShell();

            // Configure current application as the single Instance
            AppInstanceManager.SetupApplicationInstance();
            AppInstanceManager.NewInstanceStarted += OnNewInstanceStarted;

            IsInitialized = true;

            Container.Resolve<IDialogService>().ShowDialog(
                LocalIdentifiers.Dialogs.Startup,
                null,
                () => LoaderScreenManager.Hide(),
                result =>
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
              }, LocalIdentifiers.Dialogs.StartupWindow);

            return Task.CompletedTask;
        }
        #endregion
    }
}
