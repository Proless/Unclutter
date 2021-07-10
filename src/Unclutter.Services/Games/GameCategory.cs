using System.Text.Json.Serialization;
using Unclutter.API.Converters;
using Unclutter.SDK.Models;

namespace Unclutter.Services.Games
{
    public class GameCategory : IGameCategory
    {
        public GameCategory() { }

        public GameCategory(IGameCategory category)
        {
            Id = category.Id;
            GameId = category.GameId;
            Name = category.Name;
            ParentCategoryId = category.ParentCategoryId;
        }

        [JsonPropertyName("category_id")]
        public int Id { get; set; }

        public int GameId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("parent_category")]
        [JsonConverter(typeof(ParentCategoryIdConverter))]
        public int? ParentCategoryId { get; set; }
    }
}
