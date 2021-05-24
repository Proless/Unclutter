#nullable enable
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Unclutter.API.Models;
using Unclutter.API.Models.Mods;

namespace Unclutter.API
{
    public class ModsClient : NexusClient
    {
        /// <summary>
        /// Create a new instance of the <see cref="ModsClient"/> class
        /// </summary>
        /// <param name="httpClient">The HttpClient instance to use for this endpoint</param>
        public ModsClient(HttpClient httpClient) : base(httpClient) { }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public override Action<HttpRequestMessage>? RequestConfigurator { get; set; }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public override IList<JsonConverter>? Converters => null;

        /// <summary>
        /// Retrieve a specified mod, from a specified game. Cached for 5 minutes.
        /// </summary>
        public Task<NexusMod> GetModAsync(string apiKey, string gameDomain, long modId, CancellationToken cancellationToken)
        {
            var request = RequestBuilder
                .Get($"/games/{gameDomain}/mods/{modId}.json")
                .WithAPIKey(apiKey)
                .Create();

            return ProcessRequestAsync<NexusMod>(request, cancellationToken);
        }

        /// <summary>
        /// Fetch all the mods being tracked by the user associated with the provided API Key
        /// </summary>
        public Task<IEnumerable<NexusTrackedModInfo>> GetTrackedModsAsync(string apiKey, CancellationToken cancellationToken)
        {
            var request = RequestBuilder
                .Get("/user/tracked_mods.json")
                .WithAPIKey(apiKey)
                .Create();

            return ProcessRequestAsync<IEnumerable<NexusTrackedModInfo>>(request, cancellationToken);
        }

        /// <summary>
        /// Return a list of mods that have been updated in a given period, with timestamps of their last update. Cached for 5 minutes.
        /// </summary>
        public Task<IEnumerable<NexusModUpdateInfo>> GetUpdatedModsAsync(string apiKey, string gameDomain, NexusModTimePeriod timePeriod, CancellationToken cancellationToken)
        {
            var period = timePeriod switch
            {
                NexusModTimePeriod.Day => "1d",
                NexusModTimePeriod.Week => "1w",
                NexusModTimePeriod.Month => "1m",
                _ => "1d"
            };

            var request = RequestBuilder
                .Get($"/games/{gameDomain}/mods/updated.json")
                .WithQueryParameter("period", period)
                .WithAPIKey(apiKey)
                .Create();

            return ProcessRequestAsync<IEnumerable<NexusModUpdateInfo>>(request, cancellationToken);
        }

        /// <summary>
        /// Retrieve 10 latest added mods for a specified game
        /// </summary>
        public Task<IEnumerable<NexusMod>> GetLatestAddedModsAsync(string apiKey, string gameDomain, CancellationToken cancellationToken)
        {
            var request = RequestBuilder
                .Get($"/games/{gameDomain}/mods/latest_added.json")
                .WithAPIKey(apiKey)
                .Create();

            return ProcessRequestAsync<IEnumerable<NexusMod>>(request, cancellationToken);
        }

        /// <summary>
        /// Retrieve 10 latest updated mods for a specified game
        /// </summary>
        public Task<IEnumerable<NexusMod>> GetLatestUpdatedModsAsync(string apiKey, string gameDomain, CancellationToken cancellationToken)
        {
            var request = RequestBuilder
                .Get($"/games/{gameDomain}/mods/latest_updated.json")
                .WithAPIKey(apiKey)
                .Create();

            return ProcessRequestAsync<IEnumerable<NexusMod>>(request, cancellationToken);
        }

        /// <summary>
        /// Retrieve 10 trending mods for a specified game
        /// </summary>
        public Task<IEnumerable<NexusMod>> GetTrendingModsAsync(string apiKey, string gameDomain, CancellationToken cancellationToken)
        {
            var request = RequestBuilder
                .Get($"/games/{gameDomain}/mods/trending.json")
                .WithAPIKey(apiKey)
                .Create();

            return ProcessRequestAsync<IEnumerable<NexusMod>>(request, cancellationToken);
        }

        /// <summary>
        /// Returns list of change logs for a mod
        /// </summary>
        public Task<Dictionary<string, IEnumerable<string>>> GetModChangeLogsAsync(string apiKey, string gameDomain, long modId, CancellationToken cancellationToken)
        {
            var request = RequestBuilder
                .Get($"/games/{gameDomain}/mods/{modId}/changelogs.json")
                .WithAPIKey(apiKey)
                .Create();

            return ProcessRequestAsync<Dictionary<string, IEnumerable<string>>>(request, cancellationToken);
        }

        /// <summary>
        /// Track a mod with the user associated with the provided API Key
        /// </summary>
        public Task<NexusServerReply> TrackModAsync(string apiKey, string gameDomain, long modId, CancellationToken cancellationToken)
        {
            var formData = new Dictionary<string, string>
            {
                {"mod_id", modId.ToString()}
            };

            var request = RequestBuilder
                .Post("/user/tracked_mods.json")
                .WithQueryParameter("domain_name", gameDomain)
                .WithContent(formData)
                .WithAPIKey(apiKey)
                .Create();

            return ProcessRequestAsync<NexusServerReply>(request, cancellationToken);
        }

        /// <summary>
        /// Stop tracking a mod with the user associated with the provided API Key
        /// </summary>
        public Task<NexusServerReply> UnTrackModAsync(string apiKey, string gameDomain, long modId, CancellationToken cancellationToken)
        {
            var formData = new Dictionary<string, string>
            {
                {"mod_id", modId.ToString()}
            };

            var request = RequestBuilder
                .Delete("/user/tracked_mods.json")
                .WithQueryParameter("domain_name", gameDomain)
                .WithContent(formData)
                .WithAPIKey(apiKey)
                .Create();

            return ProcessRequestAsync<NexusServerReply>(request, cancellationToken);
        }

        /// <summary>
        /// Endorse a mod
        /// </summary>
        public Task<NexusServerReply> EndorseModAsync(string apiKey, string gameDomain, long modId, string version, CancellationToken cancellationToken)
        {
            var formData = new Dictionary<string, string>
            {
                {"version", version}
            };

            var request = RequestBuilder
                .Post($"/games/{gameDomain}/mods/{modId}/endorse.json")
                .WithContent(formData)
                .WithAPIKey(apiKey)
                .Create();

            return ProcessRequestAsync<NexusServerReply>(request, cancellationToken);
        }

        /// <summary>
        /// Abstain from endorsing a mod
        /// </summary>
        public Task<NexusServerReply> AbstainEndorsingModAsync(string apiKey, string gameDomain, long modId, string version, CancellationToken cancellationToken)
        {
            var formData = new Dictionary<string, string>
            {
                {"version", version}
            };

            var request = RequestBuilder
                .Post($"/games/{gameDomain}/mods/{modId}/abstain.json")
                .WithContent(formData)
                .WithAPIKey(apiKey)
                .Create();

            return ProcessRequestAsync<NexusServerReply>(request, cancellationToken);
        }

        /// <summary>
        /// Abstain from endorsing a mod
        /// </summary>
        public Task<NexusServerReply> AbstainEndorsingModAsync(string apiKey, string gameDomain, long modId, CancellationToken cancellationToken)
        {
            var request = RequestBuilder
                .Post($"/games/{gameDomain}/mods/{modId}/abstain.json")
                .WithAPIKey(apiKey)
                .Create();

            return ProcessRequestAsync<NexusServerReply>(request, cancellationToken);
        }
    }
}
