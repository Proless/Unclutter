using Unclutter.NexusAPI.Inquirers;

namespace Unclutter.NexusAPI
{
    /// <summary>
    /// Combines all other Inquirers for all available endpoints
    /// </summary>
    public class NexusAPIInquirer : INexusAPIInquirer
    {
        /* Properties */
        public IUserInquirer User { get; }
        public IGamesInquirer Games { get; }
        public IModsInquirer Mods { get; }
        public IModFilesInquirer ModFiles { get; }
        public IRateManager RateManager { get; }
        public IColourSchemesInquirer ColourSchemes { get; }

        /// <summary>
        /// Provides the set of different other Inquirers for all available endpoints
        /// </summary>
        /// <param name="client">The NexusMods client to use</param>
        public NexusAPIInquirer(INexusAPIClient client)
        {
            RateManager = client.RateManager;
            User = new UserInquirer(client);
            Games = new GamesInquirer(client);
            Mods = new ModsInquirer(client);
            ModFiles = new ModFilesInquirer(client);
            ColourSchemes = new ColourSchemesInquirer(client);
        }
    }
}
