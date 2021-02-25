namespace Unclutter.NexusAPI
{
    // TODO:
    public class NexusAPIClientFactory : INexusAPIClientFactory
    {
        /* Fields */
        private NexusAPIClient _client;

        /* Properties */
        public INexusAPIClient Instance
        {
            get
            {
                if (_client is null || _client.IsDisposed)
                {
                    _client = new NexusAPIClient();
                    return _client;
                }

                return _client;
            }
        }

        /* Constructor */
        public NexusAPIClientFactory()
        {
            _client = new NexusAPIClient();
        }

        /* Methods */
        public void SetInstanceAPIKey(string api)
        {
            _client.UseAPIKey(api);
        }
        public INexusAPIClient CreateClient(string key)
        {
            return new NexusAPIClient(key);
        }
    }
}
