using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Unclutter.API.Converters
{
    public class ParentCategoryIdConverter : JsonConverter<int>
    {
        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var id = reader.TokenType switch
            {
                JsonTokenType.Number => reader.GetInt32(),
                _ => -1
            };

            return id;
        }

        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }
}
