using System.Text.Json.Serialization;

namespace Unclutter.API.Models
{
    public class NexusColourScheme
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("primary_colour")]
        public string PrimaryColour { get; set; }

        [JsonPropertyName("secondary_colour")]
        public string SecondaryColour { get; set; }

        [JsonPropertyName("darker_colour")]
        public string DarkerColour { get; set; }
    }
}
