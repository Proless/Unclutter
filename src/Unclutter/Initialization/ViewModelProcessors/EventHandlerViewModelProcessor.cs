using System.ComponentModel.Composition;
using Unclutter.SDK.Events;
using Unclutter.SDK.IServices;
using Unclutter.SDK.Plugins;

namespace Unclutter.Initialization.ViewModelProcessors
{
    [ExportViewModelProcessor]
    public class EventHandlerViewModelProcessor : IViewModelProcessor
    {
        /* Fields */
        private readonly IEventAggregator _eventAggregator;

        /* Constructor */
        [ImportingConstructor]
        public EventHandlerViewModelProcessor(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        /* Methods */
        public void ProcessViewModel(object viewmodel, object view)
        {
            if (viewmodel is IHandler handler && handler.AutoSubscribe)
            {
                _eventAggregator.SubscribeOnPublishedThread(handler);
            }
        }
    }
}
