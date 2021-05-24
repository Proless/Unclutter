using System;

namespace Unclutter.Services.Container
{
    public class RegisteredComponentEventArgs : EventArgs
    {
        public Type Type { get; }
        public string Name { get; }

        public RegisteredComponentEventArgs(Type type, string name)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Name = name;
        }
    }
}
