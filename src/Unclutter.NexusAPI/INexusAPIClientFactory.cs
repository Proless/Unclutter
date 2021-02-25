namespace Unclutter.NexusAPI
{
    public interface INexusAPIClientFactory
    {
        void SetInstanceAPIKey(string api);
        INexusAPIClient CreateClient(string key);
    }
}
