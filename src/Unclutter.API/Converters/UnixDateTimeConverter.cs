using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Unclutter.API.Converters
{
    public class UnixDateTimeConverter : JsonConverter<DateTimeOffset>
    {
        public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType switch
            {
                JsonTokenType.String => reader.GetDateTimeOffset(),
                JsonTokenType.Number => DateTimeOffset.FromUnixTimeSeconds(reader.GetInt64()),
                _ => DateTimeOffset.UnixEpoch
            };
        }

        public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value.ToUnixTimeSeconds());
        }
    }
}
