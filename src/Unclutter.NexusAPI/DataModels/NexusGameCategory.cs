using Newtonsoft.Json;
using Unclutter.NexusAPI.Converters;

namespace Unclutter.NexusAPI.DataModels
{
    public class NexusGameCategory
	{
		[JsonProperty("category_id")]
		public long Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("parent_category")]
		[JsonConverter(typeof(NullableLongConverter))]
		public long? ParentCategoryId { get; set; }
	}
}
