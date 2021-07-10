using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unclutter.SDK.Events;

namespace Unclutter.Services.EventAggregator
{
    public class Handler
    {
        /* Fields */
        private readonly Action<Action> _marshal;
        private readonly WeakReference<IHandler> _reference;
        private readonly Dictionary<Type, MethodInfo> _supportedHandlers = new Dictionary<Type, MethodInfo>();

        /* Properties */
        public bool IsDead => !_reference.TryGetTarget(out var handler) && handler == null;

        /* Constructor */
        public Handler(IHandler handler, Action<Action> marshal)
        {
            _marshal = marshal;
            _reference = new WeakReference<IHandler>(handler);

            var interfaces = handler.GetType().GetTypeInfo().ImplementedInterfaces
                .Where(x => x.GetTypeInfo().IsGenericType && x.GetGenericTypeDefinition() == typeof(IHandler<>));

            foreach (var @interface in interfaces)
            {
                var type = @interface.GetTypeInfo().GenericTypeArguments[0];
                var method = @interface.GetRuntimeMethod(nameof(IHandler<object>.Handle), new[] { type });

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
        public void Handle(Type eventType, object @event)
        {
            _reference.TryGetTarget(out var target);

            if (target == null)
            {
                return;
            }

            _marshal(() =>
            {
                foreach (var (eventTypeInfo, method) in _supportedHandlers)
                {
                    if (eventTypeInfo.GetTypeInfo().IsAssignableFrom(eventType.GetTypeInfo()))
                    {
                        method.Invoke(target, new[] { @event });
                    }

                }
            });
        }
        public bool Handles(Type eventType)
        {
            return _supportedHandlers.Any(pair => pair.Key.GetTypeInfo().IsAssignableFrom(eventType.GetTypeInfo()));
        }
    }
}
