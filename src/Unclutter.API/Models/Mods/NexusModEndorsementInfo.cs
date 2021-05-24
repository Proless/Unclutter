using System;
using System.Text.Json.Serialization;
using Unclutter.API.Converters;

namespace Unclutter.API.Models.Mods
{
    public class NexusModEndorsementInfo
    {
        [JsonPropertyName("endorse_status")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public NexusEndorsementStatus Status { get; set; }

        [JsonPropertyName("timestamp")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTimeOffset DateTime { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }
    }
}
