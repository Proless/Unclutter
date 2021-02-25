using System;
using Newtonsoft.Json;

namespace Unclutter.NexusAPI.DataModels
{
	public class NexusMod
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("summary")]
		public string Summary { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("picture_url")]
		public Uri PictureUrl { get; set; }

		[JsonProperty("mod_id")]
		public long ModId { get; set; }

		[JsonProperty("game_id")]
		public long GameId { get; set; }

		[JsonProperty("allow_rating")]
		public bool AllowRating { get; set; }

		[JsonProperty("domain_name")]
		public string DomainName { get; set; }

		[JsonProperty("category_id")]
		public long CategoryId { get; set; }

		[JsonProperty("version")]
		public string Version { get; set; }

		[JsonProperty("endorsement_count")]
		public long EndorsementCount { get; set; }

		[JsonProperty("created_timestamp")]
		public long CreatedTimestamp { get; set; }

		[JsonProperty("created_time")]
		public DateTimeOffset CreatedTime { get; set; }

		[JsonProperty("updated_timestamp")]
		public long UpdatedTimestamp { get; set; }

		[JsonProperty("updated_time")]
		public DateTimeOffset UpdatedTime { get; set; }

		[JsonProperty("author")]
		public string Author { get; set; }

		[JsonProperty("uploaded_by")]
		public string UploadedBy { get; set; }

		[JsonProperty("uploaded_users_profile_url")]
		public Uri UploadedUsersProfileUrl { get; set; }

		[JsonProperty("contains_adult_content")]
		public bool ContainsAdultContent { get; set; }

		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("available")]
		public bool Available { get; set; }

		[JsonProperty("user")]
		public NexusUserInfo UserInfo { get; set; }

		[JsonProperty("endorsement")]
		public NexusModEndorsement Endorsement { get; set; }
	}
}
