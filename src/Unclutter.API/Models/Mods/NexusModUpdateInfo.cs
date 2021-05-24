using System;
using System.Text.Json.Serialization;
using Unclutter.API.Converters;

namespace Unclutter.API.Models.Mods
{
    public class NexusModUpdateInfo
    {
        [JsonPropertyName("mod_id")]
        public long ModId { get; set; }

        [JsonPropertyName("latest_file_update")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTimeOffset LatestFileUpdate { get; set; }

        [JsonPropertyName("latest_mod_activity")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTimeOffset LatestModActivity { get; set; }
    }
}
