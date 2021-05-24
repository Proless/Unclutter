using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Unclutter.API.Converters
{
    public class ParentCategoryIdConverter : JsonConverter<long>
    {
        public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var id = reader.TokenType switch
            {
                JsonTokenType.Number => reader.GetInt64(),
                _ => -1L
            };

            return id;
        }

        public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }
}
