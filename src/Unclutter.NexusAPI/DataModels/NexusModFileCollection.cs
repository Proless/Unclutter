using Newtonsoft.Json;

namespace Unclutter.NexusAPI.DataModels
{
	public class NexusModFileCollection
	{
		[JsonProperty("files")]
		public NexusModFile[] ModFiles { get; set; }

		[JsonProperty("file_updates")]
		public NexusModFileUpdate[] ModFileUpdates { get; set; }
	}
}
