using System;

namespace Unclutter.SDK.Settings
{
    public interface ISettings<out T> : ISettings where T : class, new()
    {
        /// <summary>
        /// The settings object instance.
        /// </summary>
        T Instance { get; }
    }

    public interface ISettings
    {
        /// <summary>
        /// Name of the settings, which will be used as the file name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Location of the settings file.
        /// </summary>
        string Location { get; }

        /// <summary>
        /// The type of the underlying settings object instance.
        /// </summary>
        Type SettingsType { get; }

        /// <summary>
        /// Get the settings instance.
        /// </summary>
        /// <typeparam name="T">The type of the Instance.</typeparam>
        ISettings<T> GetSettings<T>() where T : class, new();

        /// <summary>
        /// Save the settings.
        /// </summary>
        void Save();

        /// <summary>
        /// Reload the settings.
        /// </summary>
        void Reload();

        /// <summary>
        /// Reload the default settings.
        /// </summary>
        void Reset();
    }
}
