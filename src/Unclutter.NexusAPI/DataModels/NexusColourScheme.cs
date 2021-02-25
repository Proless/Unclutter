using Newtonsoft.Json;

namespace Unclutter.NexusAPI.DataModels
{
	public class NexusColourScheme
	{
		[JsonProperty("id")]
		public long Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("primary_colour")]
		public string PrimaryColour { get; set; }

		[JsonProperty("secondary_colour")]
		public string SecondaryColour { get; set; }

		[JsonProperty("darker_colour")]
		public string DarkerColour { get; set; }
	}
}
