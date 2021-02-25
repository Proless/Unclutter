using Newtonsoft.Json;
using System.IO;
using Unclutter.SDK.IServices;

namespace Unclutter.Services
{
    public class JSONService : IJSONService
    {
        private readonly JsonSerializer _serializer;
        public JSONService()
        {
            _serializer = new JsonSerializer
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            };
        }
        public T DeserializeFrom<T>(string file)
        {
            using var content = new FileStream(file, FileMode.Open, FileAccess.Read);
            using var reader = new StreamReader(content);
            using var jsonReader = new JsonTextReader(reader);

            return _serializer.Deserialize<T>(jsonReader);
        }
        public T Deserialize<T>(string json)
        {
            using var stream = new StringReader(json);
            using var reader = new JsonTextReader(stream);
            return _serializer.Deserialize<T>(reader);
        }
        public T Deserialize<T>(string json, T obj)
        {
            return Deserialize<T>(json);
        }
        public void SerializeTo<T>(T obj, string file)
        {
            using var fileStream = File.CreateText(file);

            _serializer.Serialize(fileStream, obj);
        }
        public string Serialize<T>(T obj)
        {
            using var stream = new StringWriter();
            using var writer = new JsonTextWriter(stream);
            _serializer.Serialize(writer, obj);
            var json = stream.ToString();
            return json;
        }
        public void Populate<T>(string json, T obj) where T : class
        {
            using var stream = new StringReader(json);
            using var reader = new JsonTextReader(stream);
            _serializer.Populate(reader, obj);
        }
    }
}
