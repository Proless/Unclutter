using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Unclutter.SDK.Common;
using Unclutter.SDK.Events;
using Unclutter.SDK.Services;
using Unclutter.SDK.Plugins;

namespace Unclutter.Services.EventAggregator
{
    public class EventAggregator : IEventAggregator, IPluginConsumer
    {
        /* Fields */
        private readonly List<Handler> _handlers = new List<Handler>();

        /* Properties */
        public ImportOptions Options => new ImportOptions();

        [ImportMany]
        public IEnumerable<IEventSource> Sources { get; set; }

        /* Constructor */
        public EventAggregator(IPluginProvider pluginProvider)
        {
            pluginProvider.ImportPlugins(this);
        }

        /* Methods */
        public bool HandlerExistsFor(Type eventType)
        {
            lock (_handlers)
            {
                return _handlers.Any(handler => handler.Handles(eventType) && !handler.IsDead);
            }
        }

        public void Subscribe(IHandler handler)
        {
            var options = handler.HandlerOptions ?? new HandlerOptions();
            switch (options.AutoSubscribeThread)
            {
                case ThreadOption.UIThread:
                    SubscribeOnUIThread(handler);
                    break;
                case ThreadOption.BackgroundThread:
                    SubscribeOnBackgroundThread(handler);
                    break;
                default:
                    SubscribeOnPublishedThread(handler);
                    break;
            }
        }

        public void SubscribeOnPublishedThread(IHandler handler)
        {
            SubscribeCore(handler, action => action());
        }

        public void SubscribeOnBackgroundThread(IHandler handler)
        {
            SubscribeCore(handler, action => Task.Factory.StartNew(action, default, TaskCreationOptions.None, TaskScheduler.Default));
        }

        public void SubscribeOnUIThread(IHandler handler)
        {
            SubscribeCore(handler, action => UIDispatcher.OnUIThread(action));
        }

        public void Unsubscribe(IHandler handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            lock (_handlers)
            {
                var found = _handlers.FirstOrDefault(x => x.Matches(handler));

                if (found != null)
                {
                    _handlers.Remove(found);
                }
            }
        }

        public void PublishOnCurrentThread(object @event)
        {
            PublishCore(@event.GetType(), @event, action => action());
        }

        public void PublishOnBackgroundThread(object @event)
        {
            PublishCore(@event.GetType(), @event, action => Task.Factory.StartNew(action, default, TaskCreationOptions.None, TaskScheduler.Default));
        }

        public void PublishOnUIThread(object @event)
        {
            PublishCore(@event.GetType(), @event, action => UIDispatcher.OnUIThread(action));
        }

        public void OnImportsSatisfied()
        {
            foreach (var eventSource in Sources)
            {
                eventSource.EventSourceChanged += OnEventSourceChanged;
            }
        }

        public void OnEventSourceChanged(object sender, object @event)
        {
            if (sender is not IEventSource source) return;

            switch (source.PublishThread)
            {
                case ThreadOption.UIThread:
                    PublishOnUIThread(@event);
                    break;
                case ThreadOption.BackgroundThread:
                    PublishOnBackgroundThread(@event);
                    break;
                default:
                    PublishOnCurrentThread(@event);
                    break;
            }
        }

        /* Core */
        public void SubscribeCore(IHandler handler, Action<Action> marshal)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            if (marshal == null)
            {
                throw new ArgumentNullException(nameof(marshal));
            }

            lock (_handlers)
            {
                if (_handlers.Any(x => x.Matches(handler)))
                {
                    return;
                }

                _handlers.Add(new Handler(handler, marshal));
            }
        }

        public void PublishCore(Type eventType, object @event, Action<Action> marshal)
        {
            if (@event == null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            if (marshal == null)
            {
                throw new ArgumentNullException(nameof(marshal));
            }

            Handler[] toNotify;

            lock (_handlers)
            {
                toNotify = _handlers.ToArray();
            }

            marshal(() =>
            {
                foreach (var toNotifyHandler in toNotify)
                {
                    toNotifyHandler.Handle(eventType, @event);
                }

                var dead = toNotify.Where(h => h.IsDead).ToList();

                if (!dead.Any()) return;

                lock (_handlers)
                {
                    foreach (var handler in dead)
                    {
                        _handlers.Remove(handler);
                    }
                }
            });
        }
    }
}
