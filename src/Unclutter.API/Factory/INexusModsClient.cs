namespace Unclutter.API.Factory
{
    public interface INexusModsClient
    {
        /// <summary>
        /// Get or set the API Key sent with each request. Note that this doesn't override the key you provide directly to the methods.
        /// </summary>
        public string APIKey { get; set; }

        /// <summary>
        /// <inheritdoc cref="UserClient"/>
        /// </summary>
        UserClient User { get; }

        /// <summary>
        /// <inheritdoc cref="GamesClient"/>
        /// </summary>
        GamesClient Games { get; }

        /// <summary>
        /// <inheritdoc cref="ModsClient"/>
        /// </summary>
        ModsClient Mods { get; }

        /// <summary>
        /// <inheritdoc cref="ModFilesClient"/>
        /// </summary>
        ModFilesClient Files { get; }

        /// <summary>
        /// <inheritdoc cref="ColourSchemesClient"/>
        /// </summary>
        ColourSchemesClient ColourSchemes { get; }
    }
}
