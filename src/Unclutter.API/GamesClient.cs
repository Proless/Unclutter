#nullable enable
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Unclutter.API.Models.Game;

namespace Unclutter.API
{
    /// <summary>
    /// Routes specific to retrieve information regarding games
    /// </summary>
    public class GamesClient : NexusClient
    {
        /// <summary>
        /// Create a new instance of the <see cref="GamesClient"/> class
        /// </summary>
        /// <param name="httpClient">The HttpClient instance to use for this endpoint</param>
        public GamesClient(HttpClient httpClient) : base(httpClient) { }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public override Action<HttpRequestMessage>? RequestConfigurator { get; set; }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public override IList<JsonConverter>? Converters => null;

        /// <summary>
        /// Returns information about a specific game (download count, file count, categories, ...)
        /// </summary>
        public Task<NexusGame> GetGameAsync(string apiKey, string gameDomain, CancellationToken cancellationToken)
        {
            var request = RequestBuilder
                .Get($"/games/{gameDomain}.json")
                .WithAPIKey(apiKey)
                .Create();

            return ProcessRequestAsync<NexusGame>(request, cancellationToken);
        }

        /// <summary>
        /// Returns all games, optionally can also return unapproved games
        /// </summary>
        public Task<IEnumerable<NexusGame>> GetGamesAsync(string apiKey, bool includeUnapproved, CancellationToken cancellationToken)
        {
            var request = RequestBuilder
                .Get($"/games.json")
                .WithQueryParameter("include_unapproved", includeUnapproved.ToString().ToLower())
                .WithAPIKey(apiKey)
                .Create();

            return ProcessRequestAsync<IEnumerable<NexusGame>>(request, cancellationToken);
        }
    }
}
