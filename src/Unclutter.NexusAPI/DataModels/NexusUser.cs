using System;
using Newtonsoft.Json;

namespace Unclutter.NexusAPI.DataModels
{
	public class NexusUser
	{
		[JsonProperty("user_id")]
		public long UserId { get; set; }

		[JsonProperty("key")]
		public string Key { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("email")]
		public string Email { get; set; }

		[JsonProperty("profile_url")]
		public Uri ProfileAvatarUrl { get; set; }

		[JsonProperty("is_supporter")]
		public bool IsSupporter { get; set; }

		[JsonProperty("is_premium")]
		public bool IsPremium { get; set; }
	}
}
