using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using Unclutter.Modules.ViewModels;
using Unclutter.SDK.Plugins;
using Unclutter.Services;

namespace Unclutter.CoreExtensions.ViewModelProcessors
{
    [ExportViewModelProcessor]
    public class ViewEventsViewModelProcessor : IViewModelProcessor
    {
        /* Fields */
        private readonly Dictionary<FrameworkElement, BaseViewModel> _vms = new Dictionary<FrameworkElement, BaseViewModel>();

        /* Properties */
        public void ProcessViewModel(object viewmodel, object view)
        {
            if (view is FrameworkElement element && viewmodel is BaseViewModel viewmodelBase)
            {
                _vms[element] = viewmodelBase;
                element.Loaded += OnViewLoaded;
            }
        }

        /* Helpers */
        private void OnViewLoaded(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;
            if (!_vms.TryGetValue(element, out var vm)) return;

            element.Loaded -= OnViewLoaded;
            UIDispatcher.OnUIThreadAsync(vm.OnViewLoaded, DispatcherPriority.Loaded);
            _vms.Remove(element);
        }
    }
}
