using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Unclutter.NexusAPI
{
    public interface INexusAPIClient : IDisposable
    {
        IRateManager RateManager { get; }
        Task<T> ProcessRequestAsync<T>(Uri requestUri, HttpMethod method, CancellationToken cancellationToken = default, HttpContent formData = null);
    }
}
