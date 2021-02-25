using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Unclutter.NexusAPI.DataModels;
using Unclutter.NexusAPI.Internals;

namespace Unclutter.NexusAPI.Inquirers
{
    /// <summary>
    /// Routes specific to the current user assigned to this API Key
    /// </summary>
    public class UserInquirer : InquirerBase, IUserInquirer
    {
        /// <summary>
        /// Routes specific to the current user assigned to this API Key
        /// </summary>
        /// <param name="client">The NexusMods client to use for this endpoint</param>
        public UserInquirer(INexusAPIClient client) : base(client) { }

        /// <summary>
        /// Checks an API key is valid and returns the user's details
        /// </summary>
        /// <param name="cancellationToken">Enables cancellation of the HTTP request</param>
        /// <returns></returns>
        public Task<NexusUser> GetUserAsync(CancellationToken cancellationToken = default)
        {
            var requestUri = ConstructRequestUri(Routes.User.Validate);
            return Client.ProcessRequestAsync<NexusUser>(requestUri, HttpMethod.Get, cancellationToken);
        }

        /// <summary>
        /// Returns a list of all endorsements for the current user
        /// </summary>
        /// <param name="cancellationToken">Enables cancellation of the HTTP request</param>
        /// <returns></returns>
        public Task<IEnumerable<NexusUserEndorsement>> GetEndorsementsAsync(CancellationToken cancellationToken = default)
        {
            var requestUri = ConstructRequestUri(Routes.User.Endorsements);
            return Client.ProcessRequestAsync<IEnumerable<NexusUserEndorsement>>(requestUri, HttpMethod.Get, cancellationToken);
        }

        /// <summary>
        /// Fetch all the mods being tracked by the current user
        /// </summary>
        /// <param name="cancellationToken">Enables cancellation of the HTTP request</param>
        /// <returns></returns>
        public Task<IEnumerable<NexusUserTrackedMod>> GetTrackedModsAsync(CancellationToken cancellationToken = default)
        {
            var requestUri = ConstructRequestUri(Routes.User.TrackedMods_GET);
            return Client.ProcessRequestAsync<IEnumerable<NexusUserTrackedMod>>(requestUri, HttpMethod.Get, cancellationToken);
        }

        /// <summary>
        /// Track a specified mod with the current user
        /// </summary>
        /// <param name="gameDomain">The game domain name</param>
        /// <param name="modId">The mod Id</param>
        /// <param name="cancellationToken">Enables cancellation of the HTTP request</param>
        /// <returns></returns>
        public Task<NexusMessage> TrackModAsync(string gameDomain, long modId, CancellationToken cancellationToken = default)
        {
            var requestUri = ConstructRequestUri(Routes.User.TrackedMods_TRACK).AddQuery("domain_name", gameDomain);

            var formData = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("mod_id", modId.ToString()) });

            return Client.ProcessRequestAsync<NexusMessage>(requestUri, HttpMethod.Post, cancellationToken, formData);
        }

        /// <summary>
        /// Stop tracking this mod with the current user
        /// </summary>
        /// <param name="gameDomain">The game domain name</param>
        /// <param name="modId">The mod Id</param>
        /// <param name="cancellationToken">Enables cancellation of the HTTP request</param>
        /// <returns></returns>
        public Task<NexusMessage> UnTrackModAsync(string gameDomain, long modId, CancellationToken cancellationToken = default)
        {
            var requestUri = ConstructRequestUri(Routes.User.TrackedMods_UNTRACK).AddQuery("domain_name", gameDomain);

            var formData = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("mod_id", modId.ToString()) });

            return Client.ProcessRequestAsync<NexusMessage>(requestUri, HttpMethod.Delete, cancellationToken, formData);
        }
    }
}
