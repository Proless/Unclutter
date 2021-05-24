namespace Unclutter.API.Factory
{
    public interface INexusClientFactory
    {
        INexusModsClient Create();
        INexusModsClient Create(string apiKey);
    }
}
