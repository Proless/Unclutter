using Newtonsoft.Json;

namespace Unclutter.NexusAPI.DataModels
{
	public class NexusUserTrackedMod
	{
		[JsonProperty("mod_id")]
		public long ModId { get; set; }

		[JsonProperty("domain_name")]
		public string DomainName { get; set; }
	}
}
