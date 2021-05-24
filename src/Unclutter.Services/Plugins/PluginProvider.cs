using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using System.Threading.Tasks;
using Unclutter.SDK.Common;
using Unclutter.SDK.IServices;
using Unclutter.SDK.Loader;
using Unclutter.SDK.Plugins;

namespace Unclutter.Services.Plugins
{
    /// <summary>
    /// A simple plugins provider which uses VS MEF to discover and compose plugins in assemblies
    /// </summary>
    public class PluginProvider : IPluginProvider
    {
        /* Services */
        private readonly IDirectoryService _directoryService;
        private readonly IContainerExportProvider _containerExportProvider;
        private readonly ILogger _logger;

        /* Fields */
        private bool _initialized;
        private AggregateCatalog _catalog;

        /* Properties */
        public CompositionContainer Container { get; private set; }
        public ICompositionService CompositionService => Container;

        /* Constructor */
        public PluginProvider(IDirectoryService directoryService, IContainerExportProvider containerExportProvider, ILoggerProvider loggerProvider)
        {
            _directoryService = directoryService;
            _containerExportProvider = containerExportProvider;
            _logger = loggerProvider.GetInstance();
        }

        /* Methods */
        public void ImportPlugins(IPluginConsumer consumer)
        {
            var config = consumer.Options ?? new ImportOptions();

            if (!config.AutoImport) return;

            try
            {
                switch (config.ImportThread)
                {
                    case ThreadOption.UIThread:
                        UIDispatcher.OnUIThread(() => CompositionService.SatisfyImportsOnce(consumer));
                        break;
                    case ThreadOption.BackgroundThread:
                        Task.Run(() => CompositionService.SatisfyImportsOnce(consumer));
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Importing plugins failed, PluginConsumer: {PluginConsumerType}", consumer.GetType().ToString());
            }
        }

        protected async Task Initialize()
        {
            if (_initialized) return;

            await Task.Run(() =>
           {
               var pluginsCatalog = new AggregateDirectoryCatalog(_directoryService.ExtensionsDirectory);

               _catalog = new AggregateCatalog();
               _catalog.Catalogs.Add(pluginsCatalog);
               _catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetCallingAssembly()));
               _catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
               _catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetEntryAssembly() ?? throw new InvalidOperationException("The entry assembly was null, this should not happen !")));

               var exportProviders = new ExportProvider[]
               {
                   _containerExportProvider.ExportProvider
               };

               Container = new CompositionContainer(_catalog, CompositionOptions.IsThreadSafe, exportProviders);
           });

            _initialized = true;
        }

        #region ILoader
        public event Action<ProgressReport> ProgressChanged;
        public LoadOptions LoaderOptions => new LoadOptions();
        public Task Load()
        {
            ProgressChanged?.Invoke(new ProgressReport("Loading plugins..."));
            return Initialize();
        }
        #endregion
    }
}
