#nullable enable
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Unclutter.API.Models.User;

namespace Unclutter.API
{
    /// <summary>
    /// Routes specific to retrieve information regarding the user account associated with an API Key
    /// </summary>
    public class UserClient : NexusClient
    {
        /// <summary>
        /// Create a new instance of the <see cref="UserClient"/> class
        /// </summary>
        /// <param name="httpClient">The HttpClient instance to use for this endpoint</param>
        public UserClient(HttpClient httpClient) : base(httpClient) { }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public override Action<HttpRequestMessage>? RequestConfigurator { get; set; }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public override IList<JsonConverter>? Converters => null;

        /// <summary>
        /// Check if an API key is valid and returns the user's details.
        /// </summary>
        public Task<NexusUser> GetUserAsync(string apiKey, CancellationToken cancellationToken)
        {
            var request = RequestBuilder
                .Get("/users/validate.json")
                .WithAPIKey(apiKey)
                .Create();

            return ProcessRequestAsync<NexusUser>(request, cancellationToken);
        }

        /// <summary>
        /// Return a list of all endorsements for the user associated with the provided API Key
        /// </summary>
        public Task<IEnumerable<NexusUserEndorsementInfo>> GetEndorsementsAsync(string apiKey, CancellationToken cancellationToken)
        {
            var request = RequestBuilder
                .Get("/user/endorsements.json")
                .WithAPIKey(apiKey)
                .Create();

            return ProcessRequestAsync<IEnumerable<NexusUserEndorsementInfo>>(request, cancellationToken);

        }
    }
}
