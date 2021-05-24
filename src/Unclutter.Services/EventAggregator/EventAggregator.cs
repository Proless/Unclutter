using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unclutter.SDK.Common;
using Unclutter.SDK.Events;
using Unclutter.SDK.IServices;
using Unclutter.SDK.Plugins;

namespace Unclutter.Services.EventAggregator
{
    public class EventAggregator : IEventAggregator, IPluginConsumer
    {
        /* Fields */
        private readonly List<Handler> _handlers = new List<Handler>();
        private readonly List<IEventSource> _eventSources = new List<IEventSource>();

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

        public void SubscribeOnPublishedThread(IHandler handler)
        {
            SubscribeCore(handler, f => f());
        }

        public void SubscribeOnBackgroundThread(IHandler handler)
        {
            SubscribeCore(handler, f => Task.Factory.StartNew(f, default, TaskCreationOptions.None, TaskScheduler.Default));
        }

        public void SubscribeOnUIThread(IHandler handler)
        {
            SubscribeCore(handler, f =>
            {
                var taskCompletionSource = new TaskCompletionSource<bool>();

                UIDispatcher.BeginOnUIThread(async () =>
                {
                    try
                    {
                        await f();

                        taskCompletionSource.SetResult(true);
                    }
                    catch (OperationCanceledException)
                    {
                        taskCompletionSource.SetCanceled();
                    }
                    catch (Exception ex)
                    {
                        taskCompletionSource.SetException(ex);
                    }
                });

                return taskCompletionSource.Task;
            });
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

        public Task PublishOnCurrentThreadAsync(object @event, CancellationToken cancellationToken = default)
        {
            return PublishAsyncCore(@event.GetType(), @event, f => f(), cancellationToken);
        }

        public Task PublishOnBackgroundThreadAsync(object @event, CancellationToken cancellationToken = default)
        {
            return PublishAsyncCore(@event.GetType(), @event, f => Task.Factory.StartNew(f, default, TaskCreationOptions.None, TaskScheduler.Default), cancellationToken);
        }

        public Task PublishOnUIThreadAsync(object @event, CancellationToken cancellationToken = default)
        {
            return PublishAsyncCore(@event.GetType(), @event, f =>
             {
                 var taskCompletionSource = new TaskCompletionSource<bool>();

                 UIDispatcher.BeginOnUIThread(async () =>
                 {
                     try
                     {
                         await f();
                         taskCompletionSource.SetResult(true);
                     }
                     catch (OperationCanceledException)
                     {
                         taskCompletionSource.SetCanceled();
                     }
                     catch (Exception ex)
                     {
                         taskCompletionSource.SetException(ex);
                     }
                 });

                 return taskCompletionSource.Task;
             }, cancellationToken);
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
                    PublishOnUIThreadAsync(@event);
                    break;
                case ThreadOption.BackgroundThread:
                    PublishOnBackgroundThreadAsync(@event);
                    break;
                default:
                    PublishOnCurrentThreadAsync(@event);
                    break;
            }
        }

        /* Core */
        public void SubscribeCore(IHandler handler, Func<Func<Task>, Task> marshal)
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

        public Task PublishAsyncCore(Type eventType, object @event, Func<Func<Task>, Task> marshal, CancellationToken cancellationToken = default)
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

            return marshal(async () =>
            {
                var tasks = toNotify.Select(h => h.Handle(eventType, @event, cancellationToken));

                await Task.WhenAll(tasks);

                var dead = toNotify.Where(h => h.IsDead).ToList();

                if (dead.Any())
                {
                    lock (_handlers)
                    {
                        foreach (var handler in dead)
                        {
                            _handlers.Remove(handler);
                        }
                    }
                }
            });
        }
    }
}
