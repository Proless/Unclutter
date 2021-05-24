using System;
using System.Threading;
using System.Threading.Tasks;
using Unclutter.SDK.Events;

namespace Unclutter.SDK.IServices
{
    public interface IEventAggregator
    {
        bool HandlerExistsFor(Type eventType);

        void Unsubscribe(IHandler handler);
        void SubscribeOnPublishedThread(IHandler handler);
        void SubscribeOnBackgroundThread(IHandler handler);
        void SubscribeOnUIThread(IHandler handler);

        Task PublishOnCurrentThreadAsync(object @event, CancellationToken cancellationToken = default);
        Task PublishOnBackgroundThreadAsync(object @event, CancellationToken cancellationToken = default);
        Task PublishOnUIThreadAsync(object @event, CancellationToken cancellationToken = default);

        /* Core */
        Task PublishAsyncCore(Type eventType, object @event, Func<Func<Task>, Task> marshal, CancellationToken cancellationToken = default);
        void SubscribeCore(IHandler handler, Func<Func<Task>, Task> marshal);
    }
}
