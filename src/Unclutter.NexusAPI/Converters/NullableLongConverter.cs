using System;
using Newtonsoft.Json;

namespace Unclutter.NexusAPI.Converters
{
    public class NullableLongConverter : JsonConverter<long?>
    {
        public override long? ReadJson(JsonReader reader, Type objectType, long? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.Value is bool)
            {
                return null;
            }

            return Convert.ToInt64(reader.Value);
        }

        public override void WriteJson(JsonWriter writer, long? value, JsonSerializer serializer)
        {
            writer.WriteValue(value);
        }
    }
}
