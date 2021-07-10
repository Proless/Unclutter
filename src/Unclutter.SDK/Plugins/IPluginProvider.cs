using System;
using System.ComponentModel.Composition.Hosting;

namespace Unclutter.SDK.Plugins
{
    public interface IPluginProvider
    {
        CompositionContainer Container { get; }
        void ImportPlugins(IPluginConsumer consumer, Action onImported = null);
    }
}
