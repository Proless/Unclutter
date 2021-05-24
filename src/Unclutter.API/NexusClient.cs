#nullable enable
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Unclutter.API.Models;

namespace Unclutter.API
{
    /// <summary>
    /// A base Client class for the NexusMods API.
    /// </summary>
    public abstract class NexusClient
    {
        public const string API = "https://api.nexusmods.com/v1";
        public const string APIKeyHeaderName = "apikey";

        /* Fields */
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _serializerOptions;

        /// <summary>
        /// The UserAgent sent with each request to the API.
        /// </summary>
        public static string UserAgent
        {
            get
            {
                var name = $"{nameof(Unclutter)}.{nameof(API)}";
                var version = typeof(NexusClient).Assembly.GetName().Version;
                var platform = $"{RuntimeInformation.OSDescription}; {RuntimeInformation.OSArchitecture}; {RuntimeInformation.FrameworkDescription}";
                return $"{name}/{(version is null ? "" : version.ToString(3))} ({platform})";
            }
        }

        /// <summary>
        /// An <see cref="Action"/> called on each request to further customize the <see cref="HttpRequestMessage"/>s sent to the API.
        /// </summary>
        public abstract Action<HttpRequestMessage>? RequestConfigurator { get; set; }

        /// <summary>
        /// A list of custom <see cref="JsonConverter"/> to use when serializing and deserializing.
        /// </summary>
        public abstract IList<JsonConverter>? Converters { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="NexusClient"/> class.
        /// </summary>
        /// <param name="httpClient"></param>
        protected NexusClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

            _serializerOptions = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters =
                {
                    new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                }
            };
        }

        /// <summary>
        /// Process <see cref="HttpRequestMessage"/>s sent to API and handles serializing and deserializing.
        /// </summary>
        protected virtual async Task<T> ProcessRequestAsync<T>(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            T output = default!;

            RequestConfigurator?.Invoke(request);

            try
            {
                using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    output = await response.Content.DeserializeAsync<T>(GetSerializerOptions(), cancellationToken);
                }
                else
                {
                    await HandleNonSuccessStatusCode(response, cancellationToken);
                }
            }
            catch (HttpRequestException ex)
            {
                throw new APIException(ex.StatusCode, ex.Message);
            }

            return output;
        }

        /* Helpers */

        private JsonSerializerOptions GetSerializerOptions()
        {
            var options = new JsonSerializerOptions(_serializerOptions);

            if (Converters is not null)
            {
                options.Converters.AddRange(Converters);
            }

            return options;
        }

        private async Task HandleNonSuccessStatusCode(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            var reply = await response.Content.DeserializeAsync<NexusServerReply>(GetSerializerOptions(), cancellationToken);
            throw new APIException(response.StatusCode, reply.Message);
        }
    }
}
