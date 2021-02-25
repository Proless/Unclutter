using Newtonsoft.Json;

namespace Unclutter.NexusAPI.DataModels
{
    public class NexusFileHashResult
	{
		[JsonProperty("mod")]
		public NexusMod Mod { get; set; }

		[JsonProperty("file_details")]
		public NexusModFile ModFile { get; set; }
	}
}
