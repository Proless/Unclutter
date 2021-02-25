using System.Collections.Generic;

namespace Unclutter.Services.Settings
{
    public interface ISettingsSectionProvider
    {
        /// <summary>
        /// Get a settings sub-section with the specified key.
        /// </summary>
        /// <param name="key">The key of the settings section.</param>
        /// <returns>The <see cref="ISettingsSection"/> or null.</returns>
        /// <remarks>
        ///     This method will return <c>null</c> if the key already exists but doesn't correspond to a sub-Section.
        ///     <br/>If key doesn't exist and no a matching sub-section is found with the specified key,
        ///     an empty <see cref="ISettingsSection"/> will be returned.
        /// </remarks>
        ISettingsSection GetSection(string key);

        /// <summary>
        /// Get an <see cref="ISettingsSection"/> by key as the specified .Net type.
        /// </summary>
        /// <typeparam name="T">The .Net type.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value to return if the section is not found.</param>
        /// <returns>The .Net object.</returns>
        T GetSection<T>(string key, T defaultValue = default) where T : class, new();

        /// <summary>
        /// Get the immediate descendant settings sub-sections.
        /// </summary>
        IEnumerable<ISettingsSection> Sections { get; }
    }
}
