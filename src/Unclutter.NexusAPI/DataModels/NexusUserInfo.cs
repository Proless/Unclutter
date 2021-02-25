using Newtonsoft.Json;

namespace Unclutter.NexusAPI.DataModels
{
	public class NexusUserInfo
	{
		[JsonProperty("member_id")]
		public long MemberId { get; set; }

		[JsonProperty("member_group_id")]
		public long MemberGroupId { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }
	}
}
