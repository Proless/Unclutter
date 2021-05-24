using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Unclutter.SDK.Common;
using Unclutter.SDK.Events;

namespace Unclutter.Services.EventAggregator
{
    public class Handler
    {
        /* Fields */
        private readonly Func<Func<Task>, Task> _marshal;
        private readonly WeakReference<IHandler> _reference;
        private readonly Dictionary<Type, MethodInfo> _supportedHandlers = new Dictionary<Type, MethodInfo>();

        /* Properties */
        public bool IsDead => !_reference.TryGetTarget(out var handler) && handler == null;

        /* Constructor */
        public Handler(IHandler handler, Func<Func<Task>, Task> marshal)
        {
            _marshal = marshal;
            _reference = new WeakReference<IHandler>(handler);

            var interfaces = handler.GetType().GetTypeInfo().ImplementedInterfaces
                .Where(x => x.GetTypeInfo().IsGenericType && x.GetGenericTypeDefinition() == typeof(IHandler<>));

            foreach (var @interface in interfaces)
            {
                var type = @interface.GetTypeInfo().GenericTypeArguments[0];
                var method = @interface.GetRuntimeMethod("HandleAsync", new[] { type, typeof(CancellationToken) });

                if (method != null)
                {
                    _supportedHandlers[type] = method;
                }
            }
        }

        /* Methods */
        public bool Matches(IHandler instance)
        {
            return _reference.TryGetTarget(out var handler) && handler == instance;
        }
        public Task Handle(Type eventType, object @event, CancellationToken cancellationToken)
        {
            _reference.TryGetTarget(out var target);

            if (target == null)
            {
                return Task.FromResult(false);
            }

            return _marshal(() =>
            {
                var tasks = _supportedHandlers
                    .Where(handler => handler.Key.GetTypeInfo().IsAssignableFrom(eventType.GetTypeInfo()))
                    .Select(pair => pair.Value.Invoke(target, new[] { @event, cancellationToken }))
                    .Select(result => (Task)result)
                    .ToList();

                return Task.WhenAll(tasks);
            });
        }
        public bool Handles(Type eventType)
        {
            return _supportedHandlers.Any(pair => pair.Key.GetTypeInfo().IsAssignableFrom(eventType.GetTypeInfo()));
        }
    }
}
