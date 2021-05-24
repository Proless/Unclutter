using System;
using System.Text.Json.Serialization;

namespace Unclutter.API.Models.Mods
{
    public class NexusMod
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("summary")]
        public string Summary { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("picture_url")]
        public Uri PictureUri { get; set; }

        [JsonPropertyName("uid")]
        public long UId { get; set; }

        [JsonPropertyName("mod_id")]
        public long ModId { get; set; }

        [JsonPropertyName("game_id")]
        public long GameId { get; set; }

        [JsonPropertyName("allow_rating")]
        public bool AllowRating { get; set; }

        [JsonPropertyName("domain_name")]
        public string DomainName { get; set; }

        [JsonPropertyName("category_id")]
        public long CategoryId { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("endorsement_count")]
        public long EndorsementCount { get; set; }

        [JsonPropertyName("created_timestamp")]
        public long CreatedTimestamp { get; set; }

        [JsonPropertyName("created_time")]
        public DateTimeOffset CreatedTime { get; set; }

        [JsonPropertyName("updated_timestamp")]
        public long UpdatedTimestamp { get; set; }

        [JsonPropertyName("updated_time")]
        public DateTimeOffset UpdatedTime { get; set; }

        [JsonPropertyName("author")]
        public string Author { get; set; }

        [JsonPropertyName("uploaded_by")]
        public string UploaderName { get; set; }

        [JsonPropertyName("uploaded_users_profile_url")]
        public Uri UploaderProfileUri { get; set; }

        [JsonPropertyName("contains_adult_content")]
        public bool ContainsAdultContent { get; set; }

        [JsonPropertyName("status")]
        public NexusModStatus Status { get; set; }

        [JsonPropertyName("available")]
        public bool Available { get; set; }

        [JsonPropertyName("user")]
        public NexusUploaderInfo UploaderInfo { get; set; }

        [JsonPropertyName("endorsement")]
        public NexusModEndorsementInfo Endorsement { get; set; }
    }
}
