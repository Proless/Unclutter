using System;

namespace Unclutter.Services.Settings
{
    internal class SettingsRegistryEntry
    {
        public string Name { get; }
        public Type Type { get; }
        public bool IsProfileSpecific { get; }
        public object DefaultsAction { get; }

        public SettingsRegistryEntry(string name, Type type, bool isProfileSpecific, object defaultsAction)
        {
            Name = name;
            Type = type;
            IsProfileSpecific = isProfileSpecific;
            DefaultsAction = defaultsAction;
        }
    }
}
