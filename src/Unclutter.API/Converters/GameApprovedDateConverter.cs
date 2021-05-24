using System;
using System.Text.Json;

namespace Unclutter.API.Converters
{
    public class GameApprovedDateConverter : UnixDateTimeConverter
    {
        public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Number:
                    var nr = reader.GetInt64();
                    if (nr == 0 || nr == 1)
                    {
                        return DateTimeOffset.UnixEpoch;
                    }
                    else
                    {
                        return base.Read(ref reader, typeToConvert, options);
                    }
                default:
                    return DateTimeOffset.UnixEpoch;
            }
        }
    }
}
