using System.Text.Json.Serialization;
using Unclutter.API.Converters;
using Unclutter.SDK.IModels;

namespace Unclutter.Services.Games
{
    public class GameCategory : IGameCategory
    {
        [JsonPropertyName("category_id")]
        public long Id { get; set; }

        [JsonIgnore]
        public long GameId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("parent_category")]
        [JsonConverter(typeof(ParentCategoryIdConverter))]
        public long? ParentCategoryId { get; set; }
    }
}
