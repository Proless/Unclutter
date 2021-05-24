using System.ComponentModel.Composition.Hosting;
using Unclutter.SDK.Loader;

namespace Unclutter.SDK.Plugins
{
    public interface IPluginProvider : ILoader
    {
        CompositionContainer Container { get; }
        void ImportPlugins(IPluginConsumer consumer);
    }
}
