using Modules.DownloadsManager.Views;
using Prism.Ioc;
using Prism.Modularity;
using Unclutter.Modules;

namespace Modules.DownloadsManager
{
    [Module(ModuleName = Name, OnDemand = false)]
    [ModuleMetadata(Name, Version, Author, Description)]
    [ExportModuleView(typeof(DownloadsView), "Downloads", "downloads")]
    public class DownloadsManagerModule : IModule
    {
        #region Metadata
        public const string Name = "DownloadsManager";
        public const string Version = "1.0";
        public const string Author = "Proless";
        public const string Description = "A downloads manager module to handle mod files downloads";
        #endregion

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }
    }
}
