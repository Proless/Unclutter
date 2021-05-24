using System.Text.Json.Serialization;

namespace Unclutter.API.Models.Mods
{
    public class NexusUploaderInfo
    {
        [JsonPropertyName("member_id")]
        public long MemberId { get; set; }

        [JsonPropertyName("member_group_id")]
        public long MemberGroupId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
