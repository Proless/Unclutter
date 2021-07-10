using Prism.Ioc;
using Prism.Ioc.Internals;
using System;

namespace Unclutter.Services.Container
{
    public abstract class AdapterContainerExtensions<TContainer> : IContainerAdapter, IContainerExtension<TContainer>, IContainerInfo where TContainer : class
    {
        /* Fields */
        private readonly IContainerExtension<TContainer> _containerExtension;
        private readonly IContainerInfo _containerInfo;

        /* Constructor */
        protected AdapterContainerExtensions(IContainerExtension<TContainer> containerExtension, IContainerInfo containerInfo)
        {
            _containerExtension = containerExtension ?? throw new ArgumentNullException(nameof(containerExtension));
            _containerInfo = containerInfo ?? throw new ArgumentNullException(nameof(containerInfo));

            // Not available to MEF Exports
            RegisterInstance(typeof(IContainerExtension), this);
            RegisterInstance(typeof(IContainerProvider), this);
            RegisterInstance(typeof(IContainerAdapter), this);
        }

        #region IContainerAdapter
        public event Action<RegisteredComponentEventArgs> ComponentRegistered;
        object IContainerAdapter.Resolve(Type type) => Resolve(type);
        object IContainerAdapter.Resolve(Type type, string name) => Resolve(type, name);
        void IContainerAdapter.Register(Type type, Func<object> factory)
        {
            _containerExtension.Register(type, factory);
        }
        protected virtual void OnComponentRegistered(Type type) => OnComponentRegistered(type, null);
        protected virtual void OnComponentRegistered(Type type, string name)
        {
            ComponentRegistered?.Invoke(new RegisteredComponentEventArgs(type, name));
        }
        #endregion

        #region IContainerProvider
        public IScopedProvider CurrentScope => _containerExtension.CurrentScope;
        public object Resolve(Type type)
        {
            return _containerExtension.Resolve(type);
        }
        public object Resolve(Type type, params (Type Type, object Instance)[] parameters)
        {
            return _containerExtension.Resolve(type, parameters);
        }
        public object Resolve(Type type, string name)
        {
            return _containerExtension.Resolve(type, name);
        }
        public object Resolve(Type type, string name, params (Type Type, object Instance)[] parameters)
        {
            return _containerExtension.Resolve(type, name, parameters);
        }
        public IScopedProvider CreateScope()
        {
            return _containerExtension.CreateScope();
        }
        #endregion

        #region IContainerRegistry
        public IContainerRegistry RegisterInstance(Type type, object instance)
        {
            _containerExtension.RegisterInstance(type, instance);

            OnComponentRegistered(type);

            return this;
        }
        public IContainerRegistry RegisterInstance(Type type, object instance, string name)
        {
            _containerExtension.RegisterInstance(type, instance, name);

            OnComponentRegistered(type, name);

            return this;
        }
        public IContainerRegistry RegisterSingleton(Type @from, Type to)
        {
            _containerExtension.RegisterSingleton(@from, to);

            OnComponentRegistered(@from);

            return this;
        }
        public IContainerRegistry RegisterSingleton(Type @from, Type to, string name)
        {
            _containerExtension.RegisterSingleton(@from, to, name);

            OnComponentRegistered(@from, name);

            return this;
        }
        public IContainerRegistry RegisterSingleton(Type type, Func<object> factoryMethod)
        {
            _containerExtension.RegisterSingleton(type, factoryMethod);

            OnComponentRegistered(type);

            return this;
        }
        public IContainerRegistry RegisterSingleton(Type type, Func<IContainerProvider, object> factoryMethod)
        {
            _containerExtension.RegisterSingleton(type, factoryMethod);

            OnComponentRegistered(type);

            return this;
        }
        public IContainerRegistry RegisterManySingleton(Type type, params Type[] serviceTypes)
        {
            _containerExtension.RegisterManySingleton(type, serviceTypes);

            foreach (var serviceType in serviceTypes)
            {
                OnComponentRegistered(serviceType);
            }

            return this;
        }
        public IContainerRegistry Register(Type @from, Type to)
        {
            _containerExtension.Register(@from, to);

            OnComponentRegistered(@from);

            return this;
        }
        public IContainerRegistry Register(Type @from, Type to, string name)
        {
            _containerExtension.Register(@from, to, name);

            OnComponentRegistered(@from, name);

            return this;
        }
        public IContainerRegistry Register(Type type, Func<object> factoryMethod)
        {
            _containerExtension.Register(type, factoryMethod);

            OnComponentRegistered(type);

            return this;
        }
        public IContainerRegistry Register(Type type, Func<IContainerProvider, object> factoryMethod)
        {
            _containerExtension.Register(type, factoryMethod);

            OnComponentRegistered(type);

            return this;
        }
        public IContainerRegistry RegisterMany(Type type, params Type[] serviceTypes)
        {
            _containerExtension.RegisterMany(type, serviceTypes);

            foreach (var serviceType in serviceTypes)
            {
                OnComponentRegistered(serviceType);
            }

            return this;
        }
        public IContainerRegistry RegisterScoped(Type @from, Type to)
        {
            _containerExtension.RegisterScoped(@from, to);

            OnComponentRegistered(@from);

            return this;
        }
        public IContainerRegistry RegisterScoped(Type type, Func<object> factoryMethod)
        {
            _containerExtension.RegisterScoped(type, factoryMethod);

            OnComponentRegistered(type);

            return this;
        }
        public IContainerRegistry RegisterScoped(Type type, Func<IContainerProvider, object> factoryMethod)
        {
            _containerExtension.RegisterScoped(type, factoryMethod);

            OnComponentRegistered(type);

            return this;
        }
        public bool IsRegistered(Type type)
        {
            return _containerExtension.IsRegistered(type);
        }
        public bool IsRegistered(Type type, string name)
        {
            return _containerExtension.IsRegistered(type, name);
        }
        #endregion

        #region IContainerExtension
        public TContainer Instance => _containerExtension.Instance;
        public void FinalizeExtension()
        {
            _containerExtension.FinalizeExtension();
        }
        #endregion

        #region IContainerInfo
        public Type GetRegistrationType(string key)
        {
            return _containerInfo.GetRegistrationType(key);
        }
        public Type GetRegistrationType(Type serviceType)
        {
            return _containerInfo.GetRegistrationType(serviceType);
        }
        #endregion
    }
}
