namespace Unclutter.SDK.Services
{
    public interface IJsonService
    {
        T DeserializeFromFile<T>(string file);
        void SerializeToFile<T>(T obj, string file);
        T Deserialize<T>(string json);
        T Deserialize<T>(string json, T obj);
        string Serialize<T>(T obj);
    }
}