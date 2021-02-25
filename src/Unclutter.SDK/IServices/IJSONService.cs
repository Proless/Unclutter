namespace Unclutter.SDK.IServices
{
    public interface IJSONService
    {
        T DeserializeFrom<T>(string file);
        void SerializeTo<T>(T obj, string file);
        T Deserialize<T>(string json);
        T Deserialize<T>(string json, T obj);
        string Serialize<T>(T obj);
        void Populate<T>(string json, T obj) where T : class;
    }
}