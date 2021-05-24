using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Unclutter.SDK.IServices;

namespace Unclutter.Services
{
    public class JsonService : IJsonService
    {
        /* Fields */
        private readonly JsonSerializerOptions _serializerOptions;

        /* Constructor */
        public JsonService()
        {
            _serializerOptions = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters =
                {
                    new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                }
            };
        }

        /* Methods */
        public T DeserializeFromFile<T>(string file)
        {
            using var content = new FileStream(file, FileMode.Open, FileAccess.Read);
            using var reader = new StreamReader(content, Encoding.UTF8);
            return JsonSerializer.Deserialize<T>(reader.ReadToEnd(), _serializerOptions);
        }

        public void SerializeToFile<T>(T obj, string file)
        {
            using var fileStream = File.CreateText(file);
            var writer = new Utf8JsonWriter(fileStream.BaseStream);
            JsonSerializer.Serialize(writer, obj, _serializerOptions);

        }

        public T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, _serializerOptions);
        }

        public T Deserialize<T>(string json, T obj)
        {
            return Deserialize<T>(json);
        }

        public string Serialize<T>(T obj)
        {
            return JsonSerializer.Serialize<T>(obj, _serializerOptions);
        }
    }
}
