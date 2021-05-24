using System.ComponentModel.Composition;
using Unclutter.SDK.Loader;
using Unclutter.SDK.Plugins;
using Unclutter.Services.Loader;
using Unclutter.ViewModels;

namespace Unclutter.Initialization.ViewModelProcessors
{
    [ExportViewModelProcessor]
    public class LoaderViewModelProcessor : IViewModelProcessor
    {
        private readonly ILoaderService _loaderService;

        [ImportingConstructor]
        public LoaderViewModelProcessor(ILoaderService loaderService)
        {
            _loaderService = loaderService;
        }

        public void ProcessViewModel(object viewmodel, object view)
        {
            switch (viewmodel)
            {
                case ShellViewModel:
                    return;
                case ILoader loader:
                    _loaderService.Load(loader);
                    break;
            }
        }
    }
}
