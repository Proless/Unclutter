using System.Net.Http;

namespace Unclutter.API
{
    public class NexusHttpClient : HttpClient
    {
        public bool IsDisposed { get; private set; }

        public NexusHttpClient() { }
        public NexusHttpClient(HttpMessageHandler handler) : base(handler) { }
        public NexusHttpClient(HttpMessageHandler handler, bool disposeHandler) : base(handler, disposeHandler) { }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            IsDisposed = true;
        }
    }
}
