using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Unclutter.NexusAPI.DataModels;
using Unclutter.NexusAPI.Internals;

namespace Unclutter.NexusAPI.Inquirers
{
    /// <summary>
    /// Routes specific to retrieve information regarding supported games
    /// </summary>
    public class GamesInquirer : InquirerBase, IGamesInquirer
    {
        /// <summary>
        /// Routes specific to retrieve information regarding supported games
        /// </summary>
        /// <param name="client">The NexusMods client to use for this endpoint</param>
        public GamesInquirer(INexusAPIClient client) : base(client) { }

        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"/> of all games, optionally can also return unapproved games
        /// </summary>
        /// <param name="includeUnapproved">Determines whether to include unapproved games or not</param>
        /// <param name="cancellationToken">Enables cancellation of the HTTP request</param>
        /// <returns></returns>
        public Task<IEnumerable<NexusGame>> GetGamesAsync(bool includeUnapproved = false, CancellationToken cancellationToken = default)
        {
            var requestUri = ConstructRequestUri(Routes.Games.Games_GET).AddQuery("include_unapproved", includeUnapproved.ToString().ToLower());
            return Client.ProcessRequestAsync<IEnumerable<NexusGame>>(requestUri, HttpMethod.Get, cancellationToken);
        }

        /// <summary>
        /// Returns a specified game, along with download count, file count and categories.
        /// </summary>
        /// <param name="gameDomain">The game domain name</param>
        /// <param name="cancellationToken">Enables cancellation of the HTTP request</param>
        /// <returns></returns>
        public Task<NexusGame> GetGameAsync(string gameDomain, CancellationToken cancellationToken = default)
        {
            var requestUri = ConstructRequestUri(Routes.Games.Game_GET, gameDomain);
            return Client.ProcessRequestAsync<NexusGame>(requestUri, HttpMethod.Get, cancellationToken);
        }
    }
}
