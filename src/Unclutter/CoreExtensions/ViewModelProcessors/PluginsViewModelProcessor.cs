using System.ComponentModel.Composition;
using Unclutter.SDK.Plugins;

namespace Unclutter.CoreExtensions.ViewModelProcessors
{
    [ExportViewModelProcessor]
    public class PluginsViewModelProcessor : IViewModelProcessor
    {
        private readonly IPluginProvider _pluginProvider;

        [ImportingConstructor]
        public PluginsViewModelProcessor(IPluginProvider pluginProvider)
        {
            _pluginProvider = pluginProvider;
        }

        public void ProcessViewModel(object viewmodel, object view)
        {
            if (viewmodel is IPluginConsumer pluginConsumer)
            {
                var options = pluginConsumer.Options ?? new ImportOptions();
                if (options.AutoImport)
                {
                    _pluginProvider.ImportPlugins(pluginConsumer);
                }
            }
        }
    }
}
