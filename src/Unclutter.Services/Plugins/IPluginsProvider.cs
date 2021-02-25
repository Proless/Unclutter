using Microsoft.VisualStudio.Composition;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Unclutter.SDK.Plugins;

namespace Unclutter.Services.Plugins
{
    public interface IPluginsProvider
    {
        ExportProvider ExportProvider { get; }
        ICompositionService CompositionService { get; }
        void ImportPlugins(IPluginsConsumer consumer);
        Task Initialize();
    }
}
