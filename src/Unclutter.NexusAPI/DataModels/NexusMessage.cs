using Newtonsoft.Json;

namespace Unclutter.NexusAPI.DataModels
{
	public class NexusMessage
	{
		[JsonProperty("message")]
		public string Message { get; set; }
		[JsonProperty("status")]
		public string Status { get; set; }
	}
}
