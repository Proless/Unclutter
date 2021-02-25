using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Unclutter.NexusAPI.DataModels;
using Unclutter.NexusAPI.Internals;

namespace Unclutter.NexusAPI.Inquirers
{
    /// <summary>
    /// Mod specific routes (E.g. retrieving latest mods, endorsing a mod)
    /// </summary>
    public class ModsInquirer : InquirerBase, IModsInquirer
    {
        /// <summary>
        /// Mod specific routes (E.g. retrieving latest mods, endorsing a mod)
        /// </summary>
        /// <param name="client">The NexusMods client to use for this endpoint</param>
        public ModsInquirer(INexusAPIClient client) : base(client) { }

        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"/> of mods that have been updated in a given period, with timestamps of their last update.
        /// </summary>
        /// <param name="gameDomain">The game domain name</param>
        /// <param name="period">The time period!</param>
        /// <param name="cancellationToken">Enables cancellation of the HTTP request</param>
        /// <returns></returns>
        public Task<IEnumerable<NexusModUpdate>> GetUpdatedMods(string gameDomain, NexusTimePeriod period, CancellationToken cancellationToken = default)
        {
            var requestUri = ConstructRequestUri(Routes.Mods.Updated, gameDomain).AddQuery("period", period.GetTimePeriod());
            return Client.ProcessRequestAsync<IEnumerable<NexusModUpdate>>(requestUri, HttpMethod.Get, cancellationToken);
        }

        /// <summary>
        /// Returns a Dictionary of all change logs for a mod
        /// </summary>
        /// <param name="gameDomain">The game domain name</param>
        /// <param name="modId">The mod Id</param>
        /// <param name="cancellationToken">Enables cancellation of the HTTP request</param>
        /// <returns></returns>
        public Task<Dictionary<string, IEnumerable<string>>> GetModChangeLogs(string gameDomain, long modId, CancellationToken cancellationToken = default)
        {
            var requestUri = ConstructRequestUri(Routes.Mods.ChangeLogs, gameDomain, modId.ToString());

            return Client.ProcessRequestAsync<Dictionary<string, IEnumerable<string>>>(requestUri, HttpMethod.Get, cancellationToken);
        }

        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"/> of the 10 latest added mods for a specified game
        /// </summary>
        /// <param name="gameDomain">The game domain name</param>
        /// <param name="cancellationToken">Enables cancellation of the HTTP request</param>
        /// <returns></returns>
        public Task<IEnumerable<NexusMod>> GetLatestAddedMods(string gameDomain, CancellationToken cancellationToken = default)
        {
            var requestUri = ConstructRequestUri(Routes.Mods.LatestAdded, gameDomain);
            return Client.ProcessRequestAsync<IEnumerable<NexusMod>>(requestUri, HttpMethod.Get, cancellationToken);
        }

        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"/> of the 10 latest updated mods for a specified game
        /// </summary>
        /// <param name="gameDomain">The game domain name</param>
        /// <param name="cancellationToken">Enables cancellation of the HTTP request</param>
        /// <returns></returns>
        public Task<IEnumerable<NexusMod>> GetLatestUpdatedMods(string gameDomain, CancellationToken cancellationToken = default)
        {
            var requestUri = ConstructRequestUri(Routes.Mods.LatestUpdated, gameDomain);
            return Client.ProcessRequestAsync<IEnumerable<NexusMod>>(requestUri, HttpMethod.Get, cancellationToken);
        }

        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"/> of the 10 trending mods for a specified game
        /// </summary>
        /// <param name="gameDomain">The game domain name</param>
        /// <param name="cancellationToken">Enables cancellation of the HTTP request</param>
        /// <returns></returns>
        public Task<IEnumerable<NexusMod>> GetLatestTrendingMods(string gameDomain, CancellationToken cancellationToken = default)
        {
            var requestUri = ConstructRequestUri(Routes.Mods.Trending, gameDomain);
            return Client.ProcessRequestAsync<IEnumerable<NexusMod>>(requestUri, HttpMethod.Get, cancellationToken);
        }

        /// <summary>
        /// Returns a specified mod, from a specified game
        /// </summary>
        /// <param name="gameDomain">The game domain name</param>
        /// <param name="modId">The mod Id to retrieve</param>
        /// <param name="cancellationToken">Enables cancellation of the HTTP request</param>
        /// <returns></returns>
        public Task<NexusMod> GetMod(string gameDomain, long modId, CancellationToken cancellationToken = default)
        {
            var requestUri = ConstructRequestUri(Routes.Mods.Mod, gameDomain, modId.ToString());
            return Client.ProcessRequestAsync<NexusMod>(requestUri, HttpMethod.Get, cancellationToken);
        }

        /// <summary>
        /// Endorse a specified mod version
        /// </summary>
        /// <param name="gameDomain">The game domain name</param>
        /// <param name="modId">The mod Id to retrieve</param>
        /// <param name="version">The mod version to endorse</param>
        /// <param name="cancellationToken">Enables cancellation of the HTTP request</param>
        /// <returns></returns>
        public Task<NexusMessage> Endorse(string gameDomain, long modId, string version, CancellationToken cancellationToken = default)
        {
            var requestUri = ConstructRequestUri(Routes.Mods.Endorse, gameDomain, modId.ToString());

            var formData = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("version", version) });

            return Client.ProcessRequestAsync<NexusMessage>(requestUri, HttpMethod.Post, cancellationToken, formData);
        }

        /// <summary>
        /// Abstain from endorsing a specified mod version
        /// </summary>
        /// <param name="gameDomain">The game domain name</param>
        /// <param name="modId">The mod Id to retrieve</param>
        /// <param name="version">The mod version to abstain from endorsing</param>
        /// <param name="cancellationToken">Enables cancellation of the HTTP request</param>
        /// <returns></returns>
        public Task<NexusMessage> Abstain(string gameDomain, long modId, string version, CancellationToken cancellationToken = default)
        {
            var requestUri = ConstructRequestUri(Routes.Mods.Abstain, gameDomain, modId.ToString());

            var formData = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("version", version) });

            return Client.ProcessRequestAsync<NexusMessage>(requestUri, HttpMethod.Post, cancellationToken, formData);
        }
    }
}
