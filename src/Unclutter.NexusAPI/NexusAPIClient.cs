using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Unclutter.NexusAPI.DataModels;
using Unclutter.NexusAPI.Internals;
using Unclutter.NexusAPI.Internals.Handlers;

namespace Unclutter.NexusAPI
{
    public class NexusAPIClient : INexusAPIClient
    {
        /* Fields */
        private readonly RateManager _internalRateManager;
        private HttpClient _client;
        private readonly string _apiKey;

        /* Properties */
        public IRateManager RateManager => _internalRateManager;
        public bool IsDisposed { get; private set; }

        /* Constructor */
        internal NexusAPIClient(string key) : this()
        {
            _apiKey = key;
        }
        internal NexusAPIClient()
        {
            _internalRateManager = new RateManager();

            IsDisposed = false;
            InitializeHttpClient();
        }

        /* Methods */
        public async Task<T> ProcessRequestAsync<T>(Uri requestUri, HttpMethod method, CancellationToken cancellationToken = default, HttpContent formData = null)
        {
            T output = default;
            var httpRequestMessage = ConstructHttpRequestMessage(requestUri, method, formData);

            using var response = await _client.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                output = await response.Content.DeserializeContent<T>();
            }

            return output;
        }
        internal void UseAPIKey(string api)
        {
            _client.DefaultRequestHeaders.Remove("apikey");
            _client.DefaultRequestHeaders.Add("apikey", api);
        }
        /* Helpers */
        private void InitializeHttpClient()
        {
            // Initialize the HttpClient handlers
            var socketsHandler = new SocketsHttpHandler
            {
                MaxConnectionsPerServer = 20,
                PooledConnectionLifetime = TimeSpan.FromMilliseconds(100),
                PooledConnectionIdleTimeout = TimeSpan.FromMilliseconds(100)
            };
            var rateLimitsProcessor = new APILimitsHandler(socketsHandler, UpdateLimits, RateManager);
            var nexusErrorsHandler = new NexusErrorsHandler(rateLimitsProcessor);

            // Initialize the HttpClient
            _client = new HttpClient(nexusErrorsHandler);

            // Configure the HttpClient
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.UserAgent.ParseAdd(ConstructUserAgent(nameof(Unclutter), Assembly.GetEntryAssembly()?.GetName().Version?.ToString(2)));
            _client.DefaultRequestHeaders.Add("apikey", _apiKey);
        }
        public HttpRequestMessage ConstructHttpRequestMessage(Uri requestUri, HttpMethod method, HttpContent httpContent = null, string acceptedMediaType = null)
        {
            var httpRequestMessage = new HttpRequestMessage
            {
                RequestUri = requestUri,
                Method = method
            };

            if (httpContent != null)
            {
                httpRequestMessage.Content = httpContent;
            }

            if (!string.IsNullOrWhiteSpace(acceptedMediaType))
            {
                httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(acceptedMediaType));
            }

            return httpRequestMessage;
        }
        private string ConstructUserAgent(string productName, string productVersion)
        {
            return $"{productName}/{productVersion} ({RuntimeInformation.OSDescription}; {RuntimeInformation.OSArchitecture})";
        }
        private string GetHeaderValue(HttpResponseMessage httpResponse, string header)
        {
            var value = "";
            if (httpResponse.Headers.TryGetValues(header, out var values))
            {
                value = values.FirstOrDefault();
            }
            return value;
        }
        private void UpdateLimits(HttpResponseMessage httpResponse)
        {
            var updatingSuccessful = TryGetLimits(httpResponse, out var limits);

            if (updatingSuccessful)
            {
                _internalRateManager.APILimits = limits;
            }
        }
        private bool TryGetLimits(HttpResponseMessage httpResponse, out APILimits limits)
        {
            try
            {
                var hLimit = int.Parse(GetHeaderValue(httpResponse, "X-RL-Hourly-Limit"));
                var hRemaining = int.Parse(GetHeaderValue(httpResponse, "X-RL-Hourly-Remaining"));
                var dLimit = int.Parse(GetHeaderValue(httpResponse, "X-RL-Daily-Limit"));
                var dRemaining = int.Parse(GetHeaderValue(httpResponse, "X-RL-Daily-Remaining"));
                var hReset = DateTime.Parse(GetHeaderValue(httpResponse, "X-RL-Hourly-Reset"));
                var dReset = DateTime.Parse(GetHeaderValue(httpResponse, "X-RL-Daily-Reset"));
                limits = new APILimits
                {
                    DailyLimit = dLimit,
                    DailyRemaining = dRemaining,
                    DailyReset = dReset,
                    HourlyLimit = hLimit,
                    HourlyRemaining = hRemaining,
                    HourlyReset = hReset
                };

                return true;
            }
            catch (Exception)
            {
                limits = null;
                return false;
            }
        }
        public void Dispose()
        {
            _client?.Dispose();
            IsDisposed = true;
        }
    }
}
