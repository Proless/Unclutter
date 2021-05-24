using System;
using System.Text.Json.Serialization;
using Unclutter.API.Converters;

namespace Unclutter.API.Models.User
{
    public class NexusUserEndorsementInfo
    {
        [JsonPropertyName("mod_id")]
        public long ModId { get; set; }

        [JsonPropertyName("domain_name")]
        public string DomainName { get; set; }

        [JsonPropertyName("date")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTimeOffset Date { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("status")]
        public NexusEndorsementStatus Status { get; set; }
    }
}
