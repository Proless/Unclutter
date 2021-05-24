#nullable enable
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Unclutter.API.Models;

namespace Unclutter.API
{
    /// <summary>
    /// Routes specific to retrieve information regarding colour-specific themes for games
    /// </summary>
    public class ColourSchemesClient : NexusClient
    {
        /// <summary>
        /// Create a new instance of the <see cref="ColourSchemesClient"/> class.
        /// </summary>
        /// <param name="httpClient">The HttpClient instance to use for this endpoint</param>
        public ColourSchemesClient(HttpClient httpClient) : base(httpClient) { }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public override Action<HttpRequestMessage>? RequestConfigurator { get; set; }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public override IList<JsonConverter>? Converters => null;

        /// <summary>
        /// Returns all colour schemes, including the primary, secondary and 'darker' colours.
        /// </summary>
        public Task<IEnumerable<NexusColourScheme>> GetColourSchemesAsync(string apiKey, CancellationToken cancellationToken)
        {
            var request = RequestBuilder
                .Get("/colourschemes.json")
                .WithAPIKey(apiKey)
                .Create();

            return ProcessRequestAsync<IEnumerable<NexusColourScheme>>(request, cancellationToken);
        }

    }
}
