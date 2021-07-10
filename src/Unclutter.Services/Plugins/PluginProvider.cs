using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using Unclutter.SDK.Common;
using Unclutter.SDK.Plugins;
using Unclutter.SDK.Services;

namespace Unclutter.Services.Plugins
{
    /// <summary>
    /// A simple plugins provider which uses MEF to discover and compose plugins
    /// </summary>
    public class PluginProvider : IPluginProvider
    {
        /* Services */
        private readonly ILogger _logger;
        private readonly IDirectoryService _directoryService;
        private readonly IContainerExportProvider _containerExportProvider;
        private bool _isInitialized;

        /* Fields */
        private AggregateCatalog _catalog;

        /* Properties */
        public CompositionContainer Container { get; private set; }
        public ICompositionService CompositionService => Container;

        /* Constructor */
        public PluginProvider(
            ILogger logger,
            IDirectoryService directoryService,
            IContainerExportProvider containerExportProvider)
        {
            _logger = logger;
            _directoryService = directoryService;
            _containerExportProvider = containerExportProvider;
        }

        /* Methods */
        public void ImportPlugins(IPluginConsumer consumer, Action onImported = null)
        {
            try
            {
                CompositionService.SatisfyImportsOnce(consumer);
                onImported?.Invoke();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Importing plugins failed, PluginConsumer: {PluginConsumerType}", consumer.GetType().FullName);
            }
        }

        public void Initialize()
        {
            if (_isInitialized) return;

            var pluginsCatalog = new AggregateDirectoryCatalog(_directoryService.ExtensionsDirectory);

            _catalog = new AggregateCatalog();
            _catalog.Catalogs.Add(pluginsCatalog);
            _catalog.Catalogs.Add(new ApplicationCatalog());

            var exportProviders = new ExportProvider[]
            {
                _containerExportProvider.ExportProvider
            };

            Container = new CompositionContainer(_catalog, CompositionOptions.IsThreadSafe, exportProviders);

            RegisterExports();

            _isInitialized = true;
        }

        /* Helpers */
        private void RegisterExports()
        {
            var ignored = new List<string>();
            var toRegister = new Dictionary<string, ExportDefinition>();
            foreach (var partDefinition in _catalog)
            {
                foreach (var exportDefinition in partDefinition.ExportDefinitions)
                {
                    var id = exportDefinition.GetExportTypeIdentity();

                    if (string.IsNullOrWhiteSpace(id) || ignored.Contains(id)) continue;

                    if (toRegister.ContainsKey(id))
                    {
                        ignored.Add(id);
                        toRegister.Remove(id);
                    }
                    else
                    {
                        toRegister.Add(id, exportDefinition);
                    }
                }
            }

            foreach (var (id, exportDefinition) in toRegister)
            {
                if (PluginServices.TryGetType(id, out var type))
                {
                    _containerExportProvider.RegisterExport(type, () => GetExportedValue(type, exportDefinition.ContractName));
                }
            }

        }
        private object GetExportedValue(Type type, string name)
        {
            var importDefinition = PluginServices.GetImportDefinition(type, name);

            if (Container.TryGetExports(importDefinition, null, out var exports) && exports != null)
            {
                var matchingExports = exports.ToArray();
                if (matchingExports.Any())
                {
                    return matchingExports.FirstOrDefault()?.Value;
                }
            }

            return null;
        }
    }
}
