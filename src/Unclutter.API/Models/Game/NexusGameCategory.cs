using System.Text.Json.Serialization;
using Unclutter.API.Converters;

namespace Unclutter.API.Models.Game
{
    public class NexusGameCategory
    {
        [JsonPropertyName("category_id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("parent_category")]
        [JsonConverter(typeof(ParentCategoryIdConverter))]
        public long ParentCategoryId { get; set; }
    }
}
