using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Unclutter.API.Factory
{
    public class NexusClientFactory : INexusClientFactory
    {
        /* Fields */
        private NexusHttpClient _httpClient;
        private SocketsHttpHandler _socketsHttpHandler;

        /* Properties */
        private HttpClient HttpClient
        {
            get
            {
                if (_httpClient is null || _httpClient.IsDisposed)
                {
                    _socketsHttpHandler = new SocketsHttpHandler
                    {
                        MaxConnectionsPerServer = 20,
                        PooledConnectionLifetime = TimeSpan.FromMilliseconds(100),
                        PooledConnectionIdleTimeout = TimeSpan.FromMilliseconds(100)
                    };
                    _httpClient = new NexusHttpClient(_socketsHttpHandler, true);
                    ConfigureClient(_httpClient);
                }
                return _httpClient;
            }
        }

        /* Methods */
        public INexusModsClient Create()
        {
            return new NexusModsClient(HttpClient);
        }
        public INexusModsClient Create(string apiKey)
        {
            var client = new NexusModsClient(HttpClient)
            {
                APIKey = apiKey
            };
            return client;
        }

        /* Helpers */
        private void ConfigureClient(HttpClient httpClient)
        {
            httpClient.Timeout = TimeSpan.FromMinutes(2);
            httpClient.BaseAddress = new Uri(NexusClient.API);
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(NexusClient.UserAgent);
        }
    }
}
