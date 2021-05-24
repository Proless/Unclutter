using System;
using System.Linq;
using System.Net.Http;

namespace Unclutter.API.Factory
{
    public class NexusModsClient : INexusModsClient
    {
        /* Properties */
        public string APIKey { get; set; }

        /* Clients */
        public UserClient User { get; }
        public GamesClient Games { get; }
        public ModsClient Mods { get; }
        public ModFilesClient Files { get; }
        public ColourSchemesClient ColourSchemes { get; }

        /* Constructor */
        public NexusModsClient(HttpClient httpClient)
        {
            if (httpClient is null)
                throw new ArgumentNullException(nameof(httpClient));

            User = new UserClient(httpClient)
            {
                RequestConfigurator = PrepareRequest
            };

            Games = new GamesClient(httpClient)
            {
                RequestConfigurator = PrepareRequest
            };

            Mods = new ModsClient(httpClient)
            {
                RequestConfigurator = PrepareRequest
            };

            Files = new ModFilesClient(httpClient)
            {
                RequestConfigurator = PrepareRequest
            };

            ColourSchemes = new ColourSchemesClient(httpClient)
            {
                RequestConfigurator = PrepareRequest
            };
        }

        /* Methods */
        public void PrepareRequest(HttpRequestMessage message)
        {
            if (string.IsNullOrWhiteSpace(APIKey)) return;

            if (message is null) return;

            if (message.Headers.TryGetValues(NexusClient.APIKeyHeaderName, out var values))
            {
                if (string.IsNullOrWhiteSpace(values.SingleOrDefault()))
                {
                    return;
                }
            }

            message.Headers.Remove(NexusClient.APIKeyHeaderName);
            message.Headers.Add(NexusClient.APIKeyHeaderName, APIKey);
        }
    }
}
