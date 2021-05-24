using System;
using Unclutter.SDK.IModels;

namespace Unclutter.SDK.Settings
{
    public interface ISettingsService
    {
        /// <summary>
        /// Register a settings type with the specified name.
        /// </summary>
        /// <typeparam name="T">The settings type.</typeparam>
        /// <param name="name">The name of the settings, which will be used to create the settings file.</param>
        /// <param name="defaults">The action used to apply default settings.</param>
        void RegisterSettings<T>(string name, Action<T> defaults = null) where T : class, new();

        /// <summary>
        /// Register a settings type with the specified name, which is specific to a <see cref="IUserProfile"/>.
        /// </summary>
        /// <typeparam name="T">The settings type.</typeparam>
        /// <param name="name">The name of the settings, which will be used to create the settings file.</param>
        /// <param name="defaults">The action used to apply default settings.</param>
        void RegisterProfileSettings<T>(string name, Action<T> defaults) where T : class, new();

        /// <summary>
        /// Retrieves the <see cref="ISettings{T}"/> instance created when registering.
        /// </summary>
        /// <typeparam name="T">The registered settings type.</typeparam>
        /// <param name="name">The settings name.</param>
        /// <returns>The <see cref="ISettings{T}"/> instance if it was registered, null otherwise.</returns>
        ISettings<T> GetSettings<T>(string name) where T : class, new();
    }
}
