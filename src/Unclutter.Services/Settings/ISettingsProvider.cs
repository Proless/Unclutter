using System.Collections.Generic;

namespace Unclutter.Services.Settings
{
    /// <summary>
    /// Provides settings key/values for an application.
    /// </summary>
    public interface ISettingsProvider
    {
        /// <summary>
        /// The count of all settings key/value pairs.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Get the root <see cref="ISettingsSection"/>.
        /// </summary>
        /// <returns>The <see cref="ISettingsSection"/> object.</returns>
        ISettingsSection GetRootSection();

        /// <summary>
        /// Returns all key/value pairs.
        /// </summary>
        /// <returns>The key/value pair settings</returns>
        IEnumerable<KeyValuePair<string, string>> Settings { get; }
    }
}
