using System;

namespace Unclutter.SDK.Settings
{
    public interface ISettings<out T> : ISettings where T : class, new()
    {
        /// <summary>
        /// The settings object instance.
        /// </summary>
        T Instance { get; }

        /// <summary>
        /// Save the settings.
        /// </summary>
        void Save();

        /// <summary>
        /// Reload the settings.
        /// </summary>
        void Reload();

        /// <summary>
        /// Reload the default settings used when registering with <see cref="ISettingsService"/>.
        /// </summary>
        void Reset();
    }

    public interface ISettings
    {
        string Name { get; }
        string Location { get; }
        Type Type { get; }
        ISettings<T> Get<T>() where T : class, new();
    }
}
