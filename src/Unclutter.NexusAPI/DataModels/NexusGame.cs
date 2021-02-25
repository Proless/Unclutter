using Newtonsoft.Json;
using System;
using Unclutter.NexusAPI.Converters;

namespace Unclutter.NexusAPI.DataModels
{
    public class NexusGame
	{
		[JsonProperty("id")]
		public long Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("forum_url")]
		public Uri ForumUrl { get; set; }

		[JsonProperty("nexusmods_url")]
		public Uri NexusmodsUrl { get; set; }

		[JsonProperty("genre")]
		public string Genre { get; set; }

		[JsonProperty("file_count")]
		public long FileCount { get; set; }

		[JsonProperty("downloads")]
		public long Downloads { get; set; }

		[JsonProperty("domain_name")]
		public string DomainName { get; set; }

		[JsonProperty("approved_date")]
		[JsonConverter(typeof(GameApprovedDateConverter))]
		public DateTimeOffset? ApprovedDate { get; set; }

		[JsonProperty("file_views")]
		public long FileViews { get; set; }

		[JsonProperty("authors")]
		public long Authors { get; set; }

		[JsonProperty("file_endorsements")]
		public long FileEndorsements { get; set; }

		[JsonProperty("mods")]
		public long Mods { get; set; }

		[JsonProperty("categories")]
		public NexusGameCategory[] Categories { get; set; }
	}
}
