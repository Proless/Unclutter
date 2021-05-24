using System.Text.Json.Serialization;

namespace Unclutter.API.Models
{
    public class NexusServerReply
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
