using Newtonsoft.Json;
using Unclutter.NexusAPI.Converters;
using Unclutter.SDK.IModels;

namespace Unclutter.Services.Games
{
    public class GameCategory : IGameCategory
    {
        [JsonProperty("category_id")]
        public long Id { get; set; }

        [JsonIgnore]
        public long GameId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("parent_category")]
        [JsonConverter(typeof(NullableLongConverter))]
        public long? ParentCategoryId { get; set; }
    }
}
