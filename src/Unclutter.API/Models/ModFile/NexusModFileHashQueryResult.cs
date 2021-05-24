using System.Text.Json.Serialization;
using Unclutter.API.Models.Mods;

namespace Unclutter.API.Models.ModFile
{
    public class NexusModFileHashQueryResult
	{
		[JsonPropertyName("mod")]
		public NexusMod Mod { get; set; }

		[JsonPropertyName("file_details")]
		public NexusModFile ModFile { get; set; }
	}
}
