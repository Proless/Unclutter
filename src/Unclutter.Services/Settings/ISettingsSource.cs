using System;

namespace Unclutter.Services.Settings
{
    /// <summary>
    /// Represents a source of settings key/values for an application.
    /// </summary>
    public interface ISettingsSource
    {
        /// <summary>
        /// The location of the source.
        /// </summary>
        Uri Location { get; }

        /// <summary>
        /// Load the <see cref="ISettingsProvider"/>.
        /// </summary>
        /// <returns>The <see cref="ISettingsProvider"/>.</returns>
        ISettingsProvider Load();

        /// <summary>
        /// Persist the settings in the underlying source represented by this <see cref="ISettingsSource"/>.
        /// </summary>
        void Persist();
    }
}
