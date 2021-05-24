using System.Collections.Generic;
using System.Windows;
using Unclutter.Modules.ViewModels;
using Unclutter.SDK.Plugins;
using Unclutter.Services;

namespace Unclutter.Initialization.ViewModelProcessors
{
    [ExportViewModelProcessor]
    public class ViewEventsViewModelProcessor : IViewModelProcessor
    {
        /* Fields */
        private Dictionary<FrameworkElement, ViewModelBase> _vms = new Dictionary<FrameworkElement, ViewModelBase>();

        /* Properties */
        public void ProcessViewModel(object viewmodel, object view)
        {
            if (view is FrameworkElement element && viewmodel is ViewModelBase viewmodelBase)
            {
                _vms[element] = viewmodelBase;
                element.Loaded += OnViewLoaded;
            }
        }

        /* Helpers */
        private void OnViewLoaded(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;
            element.Loaded -= OnViewLoaded;
            UIDispatcher.OnUIThread(_vms[element].OnViewLoaded);
            _vms.Remove(element);
        }
    }
}
