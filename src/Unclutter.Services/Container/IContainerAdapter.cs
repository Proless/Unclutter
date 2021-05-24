using System;

namespace Unclutter.Services.Container
{
    public interface IContainerAdapter
    {
        /// <summary>
        /// Event raised whenever a component gets registered in
        /// the underlying container.
        /// </summary>
        event Action<RegisteredComponentEventArgs> ComponentRegistered;

        object Resolve(Type type);
        object Resolve(Type type, string name);

        bool IsRegistered(Type type);
        bool IsRegistered(Type type, string name);
    }
}
