using System;
using Newtonsoft.Json;

namespace Unclutter.Services.Converters
{
    public class TypeConverter<TImplementation, TAbstract> : JsonConverter where TImplementation : TAbstract
    {
        public override bool CanConvert(Type objectType)
            => objectType == typeof(TAbstract);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            => serializer.Deserialize<TImplementation>(reader);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            => serializer.Serialize(writer, value);
    }
}
