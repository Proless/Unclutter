using System.Text.Json.Serialization;

namespace Unclutter.API.Models.Mods
{
    public class NexusTrackedModInfo
    {
        [JsonPropertyName("mod_id")]
        public long ModId { get; set; }

        [JsonPropertyName("domain_name")]
        public string DomainName { get; set; }
    }
}
