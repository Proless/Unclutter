using Microsoft.VisualStudio.Composition;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Threading.Tasks;
using Unclutter.SDK.IServices;
using Unclutter.SDK.Plugins;

namespace Unclutter.Services.Plugins
{
    /// <summary>
    /// A simple plugins provider which uses VS MEF to discover and compose plugins in assemblies
    /// </summary>
    public class MefPluginsProvider : IPluginsProvider
    {
        /* Fields */
        private bool _initialized;
        private readonly DirectoryCatalog _directoryCatalog;

        /* Properties */
        public ExportProvider ExportProvider { get; private set; }
        public IExportProviderFactory Factory { get; private set; }
        public ICompositionService CompositionService { get; private set; }

        /* Constructors */
        public MefPluginsProvider(IDirectoryService directoryService)
        {
            _directoryCatalog = new DirectoryCatalog(directoryService.ExtensionsDirectory);
        }

        /* Methods */
        public void ImportPlugins(IPluginsConsumer consumer)
        {
            if (consumer is null) return;

            CompositionService.SatisfyImportsOnce(consumer);
        }

        public async Task Initialize()
        {
            if (_initialized) return;

            var discovery = PartDiscovery.Combine(new AttributedPartDiscoveryV1(Resolver.DefaultInstance));

            var catalog = ComposableCatalog.Create(Resolver.DefaultInstance)
                 .AddParts(await discovery.CreatePartsAsync(Assembly.GetExecutingAssembly()))
                 .AddParts(await discovery.CreatePartsAsync(Assembly.GetCallingAssembly()))
                 .AddParts(await discovery.CreatePartsAsync(Assembly.GetEntryAssembly()))
                 .AddParts(await discovery.CreatePartsAsync(_directoryCatalog.Assemblies))
                 .WithCompositionService(); // Makes an ICompositionService export available to MEF parts to import

            // Assemble the parts into a valid graph.
            var config = CompositionConfiguration.Create(catalog);

            // Prepare an ExportProvider factory based on this graph.
            Factory = config.CreateExportProviderFactory();

            // Create an export provider, which represents a unique container of values.
            ExportProvider = Factory.CreateExportProvider();

            CompositionService = ExportProvider.GetExportedValue<ICompositionService>();

            _initialized = true;
        }
    }
}
