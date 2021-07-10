using System.ComponentModel.Composition;
using Unclutter.SDK.Events;
using Unclutter.SDK.Services;
using Unclutter.SDK.Plugins;

namespace Unclutter.CoreExtensions.ViewModelProcessors
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
            if (viewmodel is IHandler handler && AutoSubscribe(handler))
            {
                _eventAggregator.Subscribe(handler);
            }

            if (view is IHandler viewHandler && AutoSubscribe(viewHandler))
            {
                _eventAggregator.Subscribe(viewHandler);
            }
        }

        /* Helpers */
        private bool AutoSubscribe(IHandler handler)
        {
            var options = handler.HandlerOptions ?? new HandlerOptions();
            return options.AutoSubscribeToEvents;
        }
    }
}
