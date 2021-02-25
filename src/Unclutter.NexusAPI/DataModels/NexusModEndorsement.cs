using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Unclutter.NexusAPI.DataModels
{
	public class NexusModEndorsement
	{
		[JsonProperty("endorse_status")]
		public string EndorseStatus { get; set; }

		[JsonProperty("timestamp")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTimeOffset? DateTime { get; set; }

		[JsonProperty("version")]
		public string Version { get; set; }
	}
}
