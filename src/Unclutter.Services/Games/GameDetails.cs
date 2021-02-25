using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Unclutter.NexusAPI.Converters;
using Unclutter.SDK.IModels;
using Unclutter.Services.Converters;

namespace Unclutter.Services.Games
{
    public class GameDetails : IGameDetails
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("downloads")]
        public long Downloads { get; set; }

        [JsonProperty("mods")]
        public long Mods { get; set; }

        [JsonProperty("forum_url")]
        public Uri ForumUrl { get; set; }

        [JsonProperty("nexusmods_url")]
        public Uri NexusmodsUrl { get; set; }

        [JsonProperty("genre")]
        public string Genre { get; set; }

        [JsonProperty("file_count")]
        public long FileCount { get; set; }

        [JsonProperty("approved_date")]
        [JsonConverter(typeof(GameApprovedDateConverter))]
        public DateTimeOffset? ApprovedDate { get; set; }

        [JsonProperty("file_views")]
        public long FileViews { get; set; }

        [JsonProperty("authors")]
        public long Authors { get; set; }

        [JsonProperty("file_endorsements")]
        public long FileEndorsements { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("domain_name")]
        public string DomainName { get; set; }

        [JsonProperty("categories")]
        [JsonConverter(typeof(TypeConverter<List<GameCategory>, IEnumerable<IGameCategory>>))]
        public IEnumerable<IGameCategory> Categories { get; set; }

        [JsonIgnore]
        public object ImageSource { get; set; }
    }
}
