using System;
using Unclutter.SDK.Events;

namespace Unclutter.SDK.Services
{
    public interface IEventAggregator
    {
        bool HandlerExistsFor(Type eventType);

        void Unsubscribe(IHandler handler);
        void Subscribe(IHandler handler);
        void SubscribeOnPublishedThread(IHandler handler);
        void SubscribeOnBackgroundThread(IHandler handler);
        void SubscribeOnUIThread(IHandler handler);

        void PublishOnCurrentThread(object @event);
        void PublishOnBackgroundThread(object @event);
        void PublishOnUIThread(object @event);

        /* Core */
        void PublishCore(Type eventType, object @event, Action<Action> marshal);
        void SubscribeCore(IHandler handler, Action<Action> marshal);
    }
}
