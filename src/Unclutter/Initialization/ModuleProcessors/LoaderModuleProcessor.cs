using Prism.Modularity;
using System.ComponentModel.Composition;
using Unclutter.Modules;
using Unclutter.SDK.Loader;
using Unclutter.Services.Loader;

namespace Unclutter.Initialization.ModuleProcessors
{
    [ExportModuleProcessor]
    public class LoaderModuleProcessor : IModuleProcessor
    {
        /* Fields */
        private readonly ILoaderService _loaderService;

        /* Constructor */
        [ImportingConstructor]
        public LoaderModuleProcessor(ILoaderService loaderService)
        {
            _loaderService = loaderService;
        }

        /* Methods */
        public void ProcessModule(IModuleInfo moduleInfo, IModule instance)
        {
            if (instance is ILoader loader)
            {
                _loaderService.RegisterLoader(loader);
            }
        }
    }
}
